using Application.Common.Dtos;
using Application.Common.Models;
using Application.StudentPortal.Categories.Commands.SetPreferences;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface ICategoryManager : IManager
    {
        Task<List<CategoryDto>> FetchPreferencesAsync();
        Task<IResult<List<CategoryDto>>> GetPreferencesAsync();
        Task<IResult> SetPreferencesAsync(SetCategoryPreferencesCommand request);
    }
}