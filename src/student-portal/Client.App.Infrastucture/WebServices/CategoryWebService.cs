using Application.Common.Dtos;
using Application.Common.Models;
using Application.StudentPortal.Categories.Commands.SetPreferences;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class CategoryWebService : WebServiceBase, ICategoryWebService
    {
        public CategoryWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<List<CategoryDto>>> GetPreferencesAsync(string accessToken) => GetAsync<List<CategoryDto>>(CategoryEndpoints.GetPreferences, accessToken);
        public Task<IResult> SetPreferencesAsync(SetCategoryPreferencesCommand request, string accessToken) => PostAsync(CategoryEndpoints.SetPreferences, request, accessToken);

    }
}
