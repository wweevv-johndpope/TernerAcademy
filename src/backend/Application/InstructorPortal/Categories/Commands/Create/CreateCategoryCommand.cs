using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.Categories.Commands.Create
{
    public class CreateCategoryCommand : IRequest<Result<int>>
    {
        public string Name { get; set; }

        public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<int>>
        {
            private readonly IApplicationDbContext _dbContext;

            public CreateCategoryCommandHandler(IApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Result<int>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            {
                var isExists = await _dbContext.Categories.AsQueryable().AnyAsync(x => x.NameNormalize == request.Name.ToNormalize());

                if (isExists) return await Result<int>.FailAsync("Name is already used.");

                var category = new Category()
                {
                    Name = request.Name,
                    NameNormalize = request.Name.ToNormalize()
                };

                _dbContext.Categories.Add(category);
                await _dbContext.SaveChangesAsync();

                return await Result<int>.SuccessAsync(category.Id);
            }
        }
    }
}
