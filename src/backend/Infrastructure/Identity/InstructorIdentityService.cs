using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Ardalis.GuardClauses;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class InstructorIdentityService : IInstructorIdentityService
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IDateTime _dateTime;
        private readonly IAuthTokenService _authTokenService;

        public InstructorIdentityService(IApplicationDbContext dbContext, IDateTime dateTime, IAuthTokenService authTokenService)
        {
            _dbContext = dbContext;
            _dateTime = dateTime;
            _authTokenService = authTokenService;
        }

        public async Task<Instructor> GetAsync(int id)
        {
            Guard.Against.NegativeOrZero(id, nameof(id));

            var user = await _dbContext.Instructors.AsQueryable().FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<Instructor> GetAsync(string email)
        {
            Guard.Against.NullOrEmpty(email, nameof(email));

            var user = await _dbContext.Instructors.AsQueryable().FirstOrDefaultAsync(u => u.EmailNormalize == email.ToNormalize());

            return user;
        }


        public async Task<Result<AuthTokenHandler>> GetAuthTokenAsync(int id, DateTime? expires = null)
        {
            var user = await GetAsync(id);

            if (user == null)
                return await Result<AuthTokenHandler>.FailAsync();

            var nameIdentifer = $"{(int)UserType.INSTRUCTOR}|{user.Id}";

            var accountClaims = new Dictionary<string, string> {
                { ClaimTypes.NameIdentifier, nameIdentifer },
                { ClaimTypes.Email, user.Email }

            };
            if (expires.HasValue) { expires = DateTime.SpecifyKind(expires.Value, DateTimeKind.Utc); }
            var token = _authTokenService.GenerateToken(accountClaims, expires);
            return await Result<AuthTokenHandler>.SuccessAsync(token);
        }

        public async Task<Result<int>> CreateAsync(Instructor user)
        {
            var currentUser = await GetAsync(user.Email);

            if (currentUser != null)
                return await Result<int>.FailAsync("Email already used.");

            user.Email = user.Email.ToLower();
            user.EmailNormalize = user.Email.ToNormalize();
            user.DateRegistered = _dateTime.UtcNow;

            _dbContext.Instructors.Add(user);
            _dbContext.SaveChangesAsync().Wait();

            return await Result<int>.SuccessAsync(user.Id);
        }

        public async Task<IResult> UpdateAsync(int id, Instructor user)
        {
            var currentUser = await GetAsync(id);

            if (currentUser == null)
                return await Result.FailAsync("Account not found.");

            currentUser.Name = user.Name;
            currentUser.CompanyName = user.CompanyName;
            currentUser.Bio = user.Bio;

            _dbContext.Instructors.Update(currentUser);
            _dbContext.SaveChangesAsync().Wait();

            return await Result.SuccessAsync();
        }

        public async Task<Result<AuthTokenHandler>> LoginAsync(string email, string password)
        {
            var user = await _dbContext.Instructors.FirstOrDefaultAsync(s => s.EmailNormalize == email.ToNormalize());

            if (user != null)
            {
                var userPassword = await _dbContext.InstructorPasswords.AsQueryable().FirstOrDefaultAsync(x => x.IsCurrent && x.InstructorId == user.Id);
                if (userPassword != null)
                {
                    var passwordHash = HasherExtensions.Hash(password, userPassword.Salt, SHA512.Create());

                    if (userPassword.Digest == passwordHash.Digest)
                    {
                        var authTokenResult = await GetAuthTokenAsync(user.Id, _dateTime.UtcNow.AddDays(1));
                        if (authTokenResult.Succeeded)
                        {
                            return authTokenResult;
                        }
                    }
                }
            }

            return await Result<AuthTokenHandler>.FailAsync("You have entered an invalid email or password.");
        }

        public async Task SetPasswordAsync(int id, string password)
        {
            var currentPassword = await _dbContext.InstructorPasswords.AsQueryable().FirstOrDefaultAsync(x => x.InstructorId == id && x.IsCurrent);

            if (currentPassword != null)
            {
                currentPassword.IsCurrent = false;
                _dbContext.InstructorPasswords.Update(currentPassword);
                await _dbContext.SaveChangesAsync();
            }

            var passwordHash = HasherExtensions.Hash(password, 64, SHA512.Create());

            var newPassword = new InstructorPassword
            {
                InstructorId = id,
                Salt = passwordHash.Salt,
                Digest = passwordHash.Digest,
                IsCurrent = true
            };

            _dbContext.InstructorPasswords.Add(newPassword);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IResult> ChangePasswordAsync(int id, string currentPassword, string newPassword)
        {
            var cururentPassword = await _dbContext.InstructorPasswords.AsQueryable().FirstOrDefaultAsync(x => x.IsCurrent && x.InstructorId == id);
            if (cururentPassword == null) return await Result.FailAsync();

            var passwordHash = HasherExtensions.Hash(currentPassword, cururentPassword.Salt, SHA512.Create());

            if (cururentPassword.Digest != passwordHash.Digest) return await Result.FailAsync();

            await SetPasswordAsync(id, newPassword);
            return await Result.SuccessAsync();
        }


        public async Task<IResult> UpdateProfilePictureAsync(int id, string filename)
        {
            var user = await GetAsync(id);

            if (user == null)
                return await Result.FailAsync("Account not found.");

            user.ProfilePictureFilename = filename;

            _dbContext.Instructors.Update(user);
            _dbContext.SaveChangesAsync().Wait();

            return await Result.SuccessAsync();
        }

        public async Task<IResult> UpdateWalletAddressAsync(int id, string walletAddress)
        {
            var user = await GetAsync(id);

            if (user == null)
                return await Result.FailAsync("Account not found.");

            user.WalletAddress = walletAddress;

            _dbContext.Instructors.Update(user);
            _dbContext.SaveChangesAsync().Wait();

            return await Result.SuccessAsync();
        }
    }
}
