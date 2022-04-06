using Application.Common.Models;
using Application.InstructorPortal.Communities.Commands.Create;
using Application.InstructorPortal.Communities.Commands.Delete;
using Application.InstructorPortal.Communities.Commands.Update;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface ICommunityWebService : IWebService
    {
        Task<IResult> CreateAsync(CreateInstructorCommunityCommand request, string accessToken);
        Task<IResult> UpdateAsync(UpdateInstructorCommunityCommand request, string accessToken);
        Task<IResult> DeleteAsync(DeleteInstructorCommunityCommand request, string accessToken);
    }
}