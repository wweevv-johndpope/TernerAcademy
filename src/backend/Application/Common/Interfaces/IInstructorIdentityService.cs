using Application.Common.Models;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IInstructorIdentityService
    {
        Task<Instructor> GetAsync(int id);
        Task<Instructor> GetAsync(string email);
        Task<Result<AuthTokenHandler>> GetAuthTokenAsync(int id, DateTime? expires = null);
        Task<Result<int>> CreateAsync(Instructor user);
        Task<IResult> UpdateAsync(int id, Instructor user);
        Task<Result<AuthTokenHandler>> LoginAsync(string email, string password);
        Task SetPasswordAsync(int id, string password);
        Task<IResult> ChangePasswordAsync(int id, string currentPassword, string newPassword);
        Task<IResult> UpdateProfilePictureAsync(int id, string filename);
        Task<IResult> UpdateWalletAddressAsync(int id, string walletAddress);
    }
}