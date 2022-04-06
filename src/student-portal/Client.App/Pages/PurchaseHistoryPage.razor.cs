using Application.StudentPortal.CourseSubscriptions.Dtos;
using Client.App.Infrastructure.Managers;
using Client.App.Pages.Transactions.Modal;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Pages
{
    public partial class PurchaseHistoryPage : IPageBase
    {
        [Inject] public ICourseSubscriptionManager CourseSubscriptionManager { get; set; }

        public bool IsLoaded { get; set; }

        public List<StudentCourseSubscriptionPurchaseItemDto> Purchases { get; set; } = new();

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await FetchCourseSubscriptionsAsync();
            }
        }

        public async Task FetchCourseSubscriptionsAsync()
        {
            try
            {
                IsLoaded = false;
                var result = await _exceptionHandler.HandlerRequestTaskAsync(() => CourseSubscriptionManager.GetPurchaseHistoryAsync());
                Purchases = result.Data;
            }
            catch (ApiOkFailedException ex)
            {
                _appDialogService.ShowErrors(ex.Messages);
            }
            catch (Exception ex)
            {
                _appDialogService.ShowError(ex.Message);
            }
            finally
            {
                Purchases ??= new();
                IsLoaded = true;
                await InvokeAsync(StateHasChanged);
            }
        }

        private void InvokeTransactionPreviewerModal(string txHash)
        {
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Large };
            var parameters = new DialogParameters()
            {
                 { nameof(TransactionPreviewerModal.TxHash), txHash},
            };

            _dialogService.Show<TransactionPreviewerModal>("Transaction Details", parameters, options);
        }
    }
}