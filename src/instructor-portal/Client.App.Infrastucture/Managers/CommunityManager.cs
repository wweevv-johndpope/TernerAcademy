using Application.Common.Models;
using Application.InstructorPortal.Communities.Commands.Create;
using Application.InstructorPortal.Communities.Commands.Delete;
using Application.InstructorPortal.Communities.Commands.Update;
using Client.App.Infrastructure.WebServices;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class CommunityManager : ManagerBase, ICommunityManager
    {
        private readonly ICommunityWebService _communityWebService;

        public CommunityManager(IManagerToolkit managerToolkit, ICommunityWebService communityWebService) : base(managerToolkit)
        {
            _communityWebService = communityWebService;
        }

        public async Task<IResult> CreateAsync(CreateInstructorCommunityCommand request)
        {
            await PrepareForWebserviceCall();
            return await _communityWebService.CreateAsync(request, AccessToken);
        }

        public async Task<IResult> UpdateAsync(UpdateInstructorCommunityCommand request)
        {
            await PrepareForWebserviceCall();
            return await _communityWebService.UpdateAsync(request, AccessToken);
        }

        public async Task<IResult> DeleteAsync(DeleteInstructorCommunityCommand request)
        {
            await PrepareForWebserviceCall();
            return await _communityWebService.DeleteAsync(request, AccessToken);
        }
    }
}
