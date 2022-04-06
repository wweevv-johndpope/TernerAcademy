using Application.Common.Dtos;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.General.Queries.GetCourseData
{
    public class GetCourseDataQuery : IRequest<Result<GetCourseDataResponseDto>>
    {
        public class GetCourseDataQueryHandler : IRequestHandler<GetCourseDataQuery, Result<GetCourseDataResponseDto>>
        {
            private readonly IApplicationDbContext _dbContext;
            private readonly IMapper _mapper;

            public GetCourseDataQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<Result<GetCourseDataResponseDto>> Handle(GetCourseDataQuery request, CancellationToken cancellationToken)
            {
                var categoryData = await _dbContext.CategoryTopicViewItems.AsQueryable().ToListAsync();
                var courseLanguages = await _dbContext.CourseLanguages.AsQueryable().OrderBy(x => x.Name).ToListAsync();

                var mappedCourseLanguages = _mapper.Map<List<CourseLanguageDto>>(courseLanguages);

                return await Result<GetCourseDataResponseDto>.SuccessAsync(new GetCourseDataResponseDto()
                {
                    CategoryData = categoryData,
                    LanguageData = mappedCourseLanguages
                });
            }
        }
    }
}
