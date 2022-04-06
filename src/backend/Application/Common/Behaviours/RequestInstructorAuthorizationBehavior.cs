using Application.Common.Interfaces;
using Domain.Enums;
using MediatR;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using UnauthorizedAccessException = Application.Common.Exceptions.UnauthorizedAccessException;

namespace Application.Common.Behaviours
{
    public class RequestInstructorAuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
    {
        private readonly IAuthTokenService _authTokenService;
        private readonly ICallContext _context;
        private readonly IInstructorIdentityService _identityService;
        private readonly UserType _accessibleBy = UserType.INSTRUCTOR;

        public RequestInstructorAuthorizationBehavior(ICallContext context,
            IAuthTokenService authTokenService, IInstructorIdentityService identityService)
        {
            _context = context;
            _authTokenService = authTokenService;
            _identityService = identityService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_context.UserRequiresAuthorization && _context.AccesibleBy == _accessibleBy)
            {
                if (string.IsNullOrEmpty(_context.UserBearerAuthorizationToken)) throw new UnauthorizedAccessException();

                var authTokenResult = _authTokenService.ValidateToken(_context.UserBearerAuthorizationToken);
                if (authTokenResult.Status != AuthTokenStatus.Valid) throw new UnauthorizedAccessException();

                var nameIdentifier = authTokenResult.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (nameIdentifier == null) throw new UnauthorizedAccessException();

                var parts = nameIdentifier.Value.Split('|').ToList();

                var userType = (UserType)Enum.Parse(typeof(UserType), parts[0]);
                if (userType != _accessibleBy) throw new UnauthorizedAccessException();

                _context.UserId = Convert.ToInt32(parts[1]);

                var user = await _identityService.GetAsync(_context.UserId);

                _context.UserEmail = user.Email;
            }

            return await next();
        }
    }
}