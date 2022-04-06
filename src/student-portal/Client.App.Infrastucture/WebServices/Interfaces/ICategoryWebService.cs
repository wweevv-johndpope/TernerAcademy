using Application.Common.Dtos;
using Application.Common.Models;
using Application.StudentPortal.Categories.Commands.SetPreferences;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface ICategoryWebService : IWebService
    {
        Task<IResult<List<CategoryDto>>> GetPreferencesAsync(string accessToken);
        Task<IResult> SetPreferencesAsync(SetCategoryPreferencesCommand request, string accessToken);
    }
}