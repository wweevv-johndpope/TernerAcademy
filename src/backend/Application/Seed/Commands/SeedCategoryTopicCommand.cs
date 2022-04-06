using Application.Common.Exceptions;
using Application.Common.Models;
using Application.InstructorPortal.Categories.Commands.Create;
using Application.InstructorPortal.Categories.Queries.GetCategoryByName;
using Application.InstructorPortal.CategoryTopics.Commands.Create;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Seed.Commands
{
    public class SeedCategoryTopicCommand : IRequest<IResult>
    {
        public class SeedCategoryTopicCommandHandler : IRequestHandler<SeedCategoryTopicCommand, IResult>
        {
            private readonly IMediator _mediator;

            public SeedCategoryTopicCommandHandler(IMediator mediator)
            {
                _mediator = mediator;
            }

            public async Task<IResult> Handle(SeedCategoryTopicCommand request, CancellationToken cancellationToken)
            {
                var data = new List<SeedCategoryTopic>()
                {
                    new SeedCategoryTopic(){ CategoryName = "Web Development", Topics = new List<string>() { "JavaScript", "CSS", "React", "Angular", "Node.Js", "HTML5", "Blazor", "ASP.NET Core MVC", "PHP"  } },
                    new SeedCategoryTopic(){ CategoryName = "Data Science", Topics = new List<string>() { "Machine Learning", "Deep Learning", "Data Analysis", "Artificial Intelligence", "Statistic", "Natural Language Processing" } },
                    new SeedCategoryTopic(){ CategoryName = "Blockchain", Topics = new List<string>() {  "Solidity", "Theta", "Ethereum", "XRP", "Stratis"  } },
                    new SeedCategoryTopic(){ CategoryName = "Mobile Development", Topics = new List<string>() { "Google Flutter", "Android Development", "iOS Development", "Swift", "React Native", "Xamarin", "Swift UI", "Kotlin" } }
                };

                foreach (var row in data)
                {
                    var categoryId = SetAndSelectCategoryAsync(row.CategoryName).Result;

                    foreach (var topic in row.Topics)
                    {
                        SetCategoryTopicAsync(categoryId, topic).Wait();
                    }
                }

                return await Result.SuccessAsync();
            }

            public async Task<int> SetAndSelectCategoryAsync(string name)
            {
                var getResult = await _mediator.Send(new GetCategoryByNameQuery() { Name = name });

                if (!getResult.Succeeded)
                {
                    var createResult = await _mediator.Send(new CreateCategoryCommand() { Name = name });
                    if (!createResult.Succeeded) throw new NotFoundException();
                    return createResult.Data;
                }

                return getResult.Data.Id;
            }

            public async Task SetCategoryTopicAsync(int categoryId, string name)
            {
                await _mediator.Send(new CreateCategoryTopicCommand() { CategoryId = categoryId, Name = name });
            }
        }

        public class SeedCategoryTopic
        {
            public string CategoryName { get; set; }
            public List<string> Topics { get; set; } = new List<string>();
        }
    }
}
