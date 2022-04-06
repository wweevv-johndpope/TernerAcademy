using Application.Common.Dtos;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.Wallets.Queries.Get
{
    public class GetWalletQuery : IRequest<Result<WalletInfoDto>>
    {
        public class GetWalletQueryHandler : IRequestHandler<GetWalletQuery, Result<WalletInfoDto>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IRpcApiService _rpcApiService;
            public GetWalletQueryHandler(IRpcApiService rpcApiService, IConfiguration configuration, ICallContext context, IApplicationDbContext dbContext)
            {
                _context = context;
                _rpcApiService = rpcApiService;
                _dbContext = dbContext;
            }

            public async Task<Result<WalletInfoDto>> Handle(GetWalletQuery request, CancellationToken cancellationToken)
            {
                var user = await _dbContext.Instructors.AsQueryable().FirstAsync(x => x.Id == _context.UserId);

                double balance = 0;

                if (!string.IsNullOrEmpty(user.WalletAddress))
                {
                    var response = _rpcApiService.GetBalance(user.WalletAddress);

                    if (response.Error != null) return await Result<WalletInfoDto>.FailAsync(response.Error.Message);

                    balance = response.Result.ToTFuel();
                }

                return await Result<WalletInfoDto>.SuccessAsync(new WalletInfoDto()
                {
                    Address = user.WalletAddress,
                    Balance = balance
                });
            }
        }
    }
}
