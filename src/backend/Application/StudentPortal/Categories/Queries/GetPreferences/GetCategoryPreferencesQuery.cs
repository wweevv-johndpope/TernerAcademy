using Application.Common.Dtos;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.StudentPortal.Categories.Queries.GetPreferences
{
    public class GetCategoryPreferencesQuery : IRequest<Result<List<CategoryDto>>>
    {
        public class GetCategoryPreferencesQueryHandler : IRequestHandler<GetCategoryPreferencesQuery, Result<List<CategoryDto>>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IMapper _mapper;
            public GetCategoryPreferencesQueryHandler(ICallContext context, IApplicationDbContext dbContext, IMapper mapper)
            {
                _context = context;
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<Result<List<CategoryDto>>> Handle(GetCategoryPreferencesQuery request, CancellationToken cancellationToken)
            {
                var categories = await _dbContext.StudentCategoryPreferences.AsQueryable().Include(x => x.Category)
                    .Where(x => x.StudentId == _context.UserId)
                    .Select(x => x.Category)
                    .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return await Result<List<CategoryDto>>.SuccessAsync(categories);
            }
        }
    }
}
