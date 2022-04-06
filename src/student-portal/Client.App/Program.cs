using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Client.App.Extensions;
using System.Threading.Tasks;
using Client.Infrastructure.Authentication;
using Microsoft.Extensions.Logging;

namespace Client.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTM3Mzg0QDMxMzkyZTMzMmUzMExCNHpoQ2JKSklHc1hFYThGbDdnbElnQmpER3hhZWtNdUhsNEhOVlgwUjQ9");

            var builder = WebAssemblyHostBuilder
                    .CreateDefault(args)
                    .AddRootComponents()
                    .AddClientServices();

            var host = builder.Build();

            var routeViewService = host.Services.GetRequiredService<AppRouteViewService>();
            await routeViewService.Populate();
           
            await host.RunAsync();
        }
    }
}
