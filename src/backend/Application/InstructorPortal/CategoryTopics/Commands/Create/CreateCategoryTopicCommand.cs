using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.CategoryTopics.Commands.Create
{
    public class CreateCategoryTopicCommand : IRequest<IResult>
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public class CreateCategoryTopicCommandHandler : IRequestHandler<CreateCategoryTopicCommand, IResult>
        {
            private readonly IApplicationDbContext _dbContext;

            public CreateCategoryTopicCommandHandler(IApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<IResult> Handle(CreateCategoryTopicCommand request, CancellationToken cancellationToken)
            {
                var isExists = await _dbContext.CategoryTopics.AsQueryable().AnyAsync(x => x.NameNormalize == request.Name.ToNormalize());

                if (isExists) return await Result.FailAsync("Name is already used.");

                var topic = new CategoryTopic()
                {
                    CategoryId = request.CategoryId,
                    Name = request.Name,
                    NameNormalize = request.Name.ToNormalize()
                };

                _dbContext.CategoryTopics.Add(topic);
                await _dbContext.SaveChangesAsync();

                return await Result<int>.SuccessAsync(topic.Id);
            }
        }
    }
}
