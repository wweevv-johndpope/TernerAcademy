using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.General.Queries.GetTFuelPrice
{
    public class GetTFuelPriceQuery : IRequest<Result<double>>
    {
        public class GetTFuelPriceQueryHandler : IRequestHandler<GetTFuelPriceQuery, Result<double>>
        {
            private readonly IThetaTokenExplorerApiService _apiService;

            public GetTFuelPriceQueryHandler(IThetaTokenExplorerApiService apiService)
            {
                _apiService = apiService;
            }

            public async Task<Result<double>> Handle(GetTFuelPriceQuery request, CancellationToken cancellationToken)
            {
                var tokenPrices = _apiService.GetTokenPrices();

                var tokenPrice = tokenPrices.FirstOrDefault(x => x.Id == "TFUEL");

                if (tokenPrice == null) return await Result<double>.SuccessAsync(data: 0);

                return await Result<double>.SuccessAsync(data: tokenPrice.Price);
            }
        }
    }
}
