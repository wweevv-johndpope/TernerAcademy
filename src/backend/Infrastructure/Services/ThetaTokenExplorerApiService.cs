using Application.Common.Constants;
using Application.Common.Dtos.Response;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System.Collections.Generic;

namespace Infrastructure.Services
{
    public class ThetaTokenExplorerApiService : IThetaTokenExplorerApiService
    {
        private readonly IRestClient _client;
        public ThetaTokenExplorerApiService(IConfiguration configuration)
        {
            var baseUrl = configuration.GetValue<string>(EnvironmentVariableKeys.THETATOKENEXPLORERENDPOINT);
            _client = new RestClient(baseUrl);
        }

        private T Execute<T>(RestRequest request)
        {
            var response = _client.Execute<T>(request);
            ProcessResponse(response);
            return response.Data;
        }


        private void ProcessResponse(IRestResponse response)
        {
            if (!response.IsSuccessful)
            {
                throw new ThetaTokenExplorerServerErrorException("There's a problem on Theta Token Explorer API Service. Please try again.");
            }
        }

        public List<ThetaTokenPriceDto> GetTokenPrices()
        {
            var request = new RestRequest("api/price/all", Method.GET, DataFormat.Json);

            try
            {
                var result = Execute<ThetaTokenExplorerApiResultDto<List<ThetaTokenPriceDto>>>(request);
                return result.Body;
            }
            catch
            {
                return new List<ThetaTokenPriceDto>();
            }
        }
    }
}
