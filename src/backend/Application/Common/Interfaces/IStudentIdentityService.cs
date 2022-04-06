using Application.Common.Models;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IStudentIdentityService
    {
        Task<Student> GetAsync(int id);
        Task<Student> GetAsync(string email);
        Task<Result<AuthTokenHandler>> GetAuthTokenAsync(int id, DateTime? expires = null);
        Task<Result<int>> CreateAsync(Student user);
        Task<IResult> UpdateAsync(int id, Student user);
        Task<Result<AuthTokenHandler>> LoginAsync(string email, string password);
        Task SetPasswordAsync(int id, string password);
        Task<IResult> ChangePasswordAsync(int id, string currentPassword, string newPassword);
        Task<IResult> UpdateProfilePictureAsync(int id, string filename);
    }
}