using Application.Common.Models;
using Application.InstructorPortal.CourseLanguages.Commands.Create;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Seed.Commands
{
    public class SeedCourseLanguageCommand : IRequest<IResult>
    {
        public class SeedCourseLanguageCommandHandler : IRequestHandler<SeedCourseLanguageCommand, IResult>
        {
            private readonly IMediator _mediator;

            public SeedCourseLanguageCommandHandler(IMediator mediator)
            {
                _mediator = mediator;
            }

            public async Task<IResult> Handle(SeedCourseLanguageCommand request, CancellationToken cancellationToken)
            {
                var data = new List<string>()
                {
                    "English", "Japanese", "Mandarin", "Filipino", "Arabic", "Spanish", "Korean", "Hindi", "French", "Italian"
                };

                foreach (var row in data)
                {
                    _mediator.Send(new CreateCourseLanguageCommand() { Name = row }).Wait();
                }

                return await Result.SuccessAsync();
            }
        }
    }
}
