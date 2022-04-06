using Application.Common.Models;
using Application.InstructorPortal.Communities.Commands.Create;
using Application.InstructorPortal.Communities.Commands.Delete;
using Application.InstructorPortal.Communities.Commands.Update;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class CommunityWebService : WebServiceBase, ICommunityWebService
    {
        public CommunityWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult> CreateAsync(CreateInstructorCommunityCommand request, string accessToken) => PostAsync(CommunityEndpoints.Create, request, accessToken);
        public Task<IResult> UpdateAsync(UpdateInstructorCommunityCommand request, string accessToken) => PostAsync(string.Format(CommunityEndpoints.Update, request.CommunityId), request, accessToken);
        public Task<IResult> DeleteAsync(DeleteInstructorCommunityCommand request, string accessToken) => PostAsync(string.Format(CommunityEndpoints.Delete, request.CommunityId), request, accessToken);
    }
}
