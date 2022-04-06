using Application.Common.Dtos;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.Categories.Queries.GetCategoryByName
{
    public class GetCategoryByNameQuery : IRequest<Result<CategoryDto>>
    {
        public string Name { get; set; }

        public class GetCategoryByNameQueryHandler : IRequestHandler<GetCategoryByNameQuery, Result<CategoryDto>>
        {
            private readonly IApplicationDbContext _dbContext;
            private readonly IMapper _mapper;
            public GetCategoryByNameQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<Result<CategoryDto>> Handle(GetCategoryByNameQuery request, CancellationToken cancellationToken)
            {
                var category = await _dbContext.Categories.AsQueryable().FirstOrDefaultAsync(x => x.NameNormalize == request.Name.ToNormalize());

                if (category == null) return await Result<CategoryDto>.FailAsync("Category doesn't exists.");

                var mappedCategory = _mapper.Map<CategoryDto>(category);
                return await Result<CategoryDto>.SuccessAsync(mappedCategory);
            }
        }
    }
}
