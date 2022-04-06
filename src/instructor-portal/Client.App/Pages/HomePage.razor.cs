using Application.InstructorPortal.Dashboard.Queries.Get;
using Client.App.Infrastructure.Managers;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;

namespace Client.App.Pages
{
    public partial class HomePage
    {
        [Inject] public IDashboardManager DashboardManager { get; set; }

        public GetDashboardResponseDto Dashboard { get; set; } = new();
        public Typo HeaderTypo { get; set; } = Typo.h5;
        public Typo NumberValueTypo { get; set; } = Typo.h4;
        public string TFuelLogoStyle { get; set; } = "height:30px; width:30px;";

        protected override void OnInitialized()
        {
            AppBreakpointService.BreakpointChanged += async (s, e) => await SetStylesAsync(e);
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await FetchDashboardAsync();
                await InvokeAsync(StateHasChanged);
                await SetStylesAsync(AppBreakpointService.CurrentBreakpoint);
                await GetDashboardAsync();
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task SetStylesAsync(Breakpoint e)
        {
            if (e == Breakpoint.Xs)
            {
                HeaderTypo = Typo.h5;
                NumberValueTypo = Typo.h4;
                TFuelLogoStyle = "height:30px; width:30px;";
            }
            else
            {
                HeaderTypo = Typo.h4;
                NumberValueTypo = Typo.h2;
                TFuelLogoStyle = "height:40px; width:40px;";
            }

            await InvokeAsync(StateHasChanged);
        }


        public async Task FetchDashboardAsync()
        {
            Dashboard = await DashboardManager.FetchDashboardAsync();
        }

        public async Task GetDashboardAsync()
        {
            try
            {
                var result = await _exceptionHandler.HandlerRequestTaskAsync(() => DashboardManager.GetDashboardAsync());
                Dashboard = result.Data;
            }
            catch { }
        }
    }
}