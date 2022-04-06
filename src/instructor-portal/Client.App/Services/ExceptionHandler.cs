using Application.Common.Localization;
using Application.Common.Models;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Services
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly NavigationManager _navigationManager;
        private readonly ISnackbar _snackBar;
        private readonly IAccountManager _accountManager;

        public ExceptionHandler(NavigationManager navigationManager, ISnackbar snackBar, IAccountManager accountManager)
        {
            _navigationManager = navigationManager;
            _snackBar = snackBar;
            _accountManager = accountManager;
        }

        public async Task HandlerRequestTaskAsync(Func<Task> task)
        {
            try
            {
                await task();
            }
            catch (SessionExpiredException)
            {
                var profile = await _accountManager.FetchProfileAsync();
                if (profile != null)
                {
                    await _accountManager.LogoutAsync();
                    _snackBar.Add(LocalizationResource.Error_SessionExpired, Severity.Error);
                    _navigationManager.NavigateTo("/auth/login");
                }
            }
        }

        public async Task<IResult> HandlerRequestTaskAsync(Func<Task<IResult>> task)
        {
            try
            {
                var result = await task();

                if (!result.Succeeded)
                {
                    throw new ApiOkFailedException(result.Messages);
                }

                return result;
            }
            catch (SessionExpiredException)
            {
                var profile = await _accountManager.FetchProfileAsync();
                if (profile != null)
                {
                    await _accountManager.LogoutAsync();
                    _snackBar.Add(LocalizationResource.Error_SessionExpired, Severity.Error);
                    _navigationManager.NavigateTo("/auth/login");
                }

                return await Result.FailAsync();
            }
        }

        public async Task<IResult<TResult>> HandlerRequestTaskAsync<TResult>(Func<Task<IResult<TResult>>> task)
        {
            try
            {
                var result = await task();

                if (!result.Succeeded)
                {
                    throw new ApiOkFailedException(result.Messages);
                }

                return result;
            }
            catch (SessionExpiredException)
            {
                var profile = await _accountManager.FetchProfileAsync();
                if (profile != null)
                {
                    await _accountManager.LogoutAsync();
                    _snackBar.Add(LocalizationResource.Error_SessionExpired, Severity.Error);
                    _navigationManager.NavigateTo("/auth/login");
                }
                return await Result<TResult>.FailAsync();
            }
        }
    }
}
