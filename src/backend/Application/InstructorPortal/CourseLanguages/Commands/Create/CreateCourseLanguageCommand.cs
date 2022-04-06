using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.CourseLanguages.Commands.Create
{
    public class CreateCourseLanguageCommand : IRequest<Result<int>>
    {
        public string Name { get; set; }

        public class CreateCourseLanguageCommandHandler : IRequestHandler<CreateCourseLanguageCommand, Result<int>>
        {
            private readonly IApplicationDbContext _dbContext;

            public CreateCourseLanguageCommandHandler(IApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Result<int>> Handle(CreateCourseLanguageCommand request, CancellationToken cancellationToken)
            {
                var isExists = await _dbContext.CourseLanguages.AsQueryable().AnyAsync(x => x.NameNormalize == request.Name.ToNormalize());

                if (isExists) return await Result<int>.FailAsync("Name is already used.");

                var language = new CourseLanguage()
                {
                    Name = request.Name,
                    NameNormalize = request.Name.ToNormalize()
                };

                _dbContext.CourseLanguages.Add(language);
                await _dbContext.SaveChangesAsync();

                return await Result<int>.SuccessAsync(language.Id);
            }
        }
    }
}
