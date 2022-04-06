using Application.Common.Dtos.Response;
using System.Collections.Generic;

namespace Application.Common.Interfaces
{
    public interface IThetaTokenExplorerApiService
    {
        List<ThetaTokenPriceDto> GetTokenPrices();
    }
}