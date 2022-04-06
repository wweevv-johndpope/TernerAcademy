using Application.Common.Models;
using Application.InstructorPortal.Communities.Commands.Create;
using Application.InstructorPortal.Communities.Commands.Delete;
using Application.InstructorPortal.Communities.Commands.Update;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface ICommunityManager : IManager
    {
        Task<IResult> CreateAsync(CreateInstructorCommunityCommand request);
        Task<IResult> UpdateAsync(UpdateInstructorCommunityCommand request);
        Task<IResult> DeleteAsync(DeleteInstructorCommunityCommand request);

    }
}