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
    public class StudentIdentityService : IStudentIdentityService
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IDateTime _dateTime;
        private readonly IAuthTokenService _authTokenService;

        public StudentIdentityService(IApplicationDbContext dbContext, IDateTime dateTime, IAuthTokenService authTokenService)
        {
            _dbContext = dbContext;
            _dateTime = dateTime;
            _authTokenService = authTokenService;
        }

        public async Task<Student> GetAsync(int id)
        {
            Guard.Against.NegativeOrZero(id, nameof(id));

            var user = await _dbContext.Students.AsQueryable().FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<Student> GetAsync(string email)
        {
            Guard.Against.NullOrEmpty(email, nameof(email));

            var user = await _dbContext.Students.AsQueryable().FirstOrDefaultAsync(u => u.EmailNormalize == email.ToNormalize());

            return user;
        }


        public async Task<Result<AuthTokenHandler>> GetAuthTokenAsync(int id, DateTime? expires = null)
        {
            var user = await GetAsync(id);

            if (user == null)
                return await Result<AuthTokenHandler>.FailAsync();

            var nameIdentifer = $"{(int)UserType.STUDENT}|{user.Id}";

            var accountClaims = new Dictionary<string, string> {
                { ClaimTypes.NameIdentifier, nameIdentifer },
                { ClaimTypes.Email, user.Email }

            };
            if (expires.HasValue) { expires = DateTime.SpecifyKind(expires.Value, DateTimeKind.Utc); }
            var token = _authTokenService.GenerateToken(accountClaims, expires);
            return await Result<AuthTokenHandler>.SuccessAsync(token);
        }

        public async Task<Result<int>> CreateAsync(Student user)
        {
            var currentUser = await GetAsync(user.Email);

            if (currentUser != null)
                return await Result<int>.FailAsync("Email already used.");

            user.Email = user.Email.ToLower();
            user.EmailNormalize = user.Email.ToNormalize();
            user.DateRegistered = _dateTime.UtcNow;

            _dbContext.Students.Add(user);
            _dbContext.SaveChangesAsync().Wait();

            return await Result<int>.SuccessAsync(user.Id);
        }

        public async Task<IResult> UpdateAsync(int id, Student user)
        {
            var currentUser = await GetAsync(id);

            if (currentUser == null)
                return await Result.FailAsync("Account not found.");

            currentUser.Name = user.Name;

            _dbContext.Students.Update(currentUser);
            _dbContext.SaveChangesAsync().Wait();

            return await Result.SuccessAsync();
        }

        public async Task<Result<AuthTokenHandler>> LoginAsync(string email, string password)
        {
            var user = await _dbContext.Students.FirstOrDefaultAsync(s => s.EmailNormalize == email.ToNormalize());

            if (user != null)
            {
                var userPassword = await _dbContext.StudentPasswords.AsQueryable().FirstOrDefaultAsync(x => x.IsCurrent && x.StudentId == user.Id);
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
            var passwordHash = HasherExtensions.Hash(password, 64, SHA512.Create());

            var currentPassword = await _dbContext.StudentPasswords.AsQueryable().FirstOrDefaultAsync(x => x.StudentId == id && x.IsCurrent);

            if (currentPassword != null)
            {
                currentPassword.IsCurrent = false;
                await _dbContext.SaveChangesAsync();
            }

            var newPassword = new StudentPassword
            {
                StudentId = id,
                Salt = passwordHash.Salt,
                Digest = passwordHash.Digest,
                IsCurrent = true
            };

            _dbContext.StudentPasswords.Add(newPassword);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IResult> ChangePasswordAsync(int id, string currentPassword, string newPassword)
        {
            var cururentPassword = await _dbContext.StudentPasswords.AsQueryable().FirstOrDefaultAsync(x => x.IsCurrent && x.StudentId == id);
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

            _dbContext.Students.Update(user);
            _dbContext.SaveChangesAsync().Wait();

            return await Result.SuccessAsync();
        }
    }
}
