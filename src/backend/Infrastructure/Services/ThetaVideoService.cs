using Application.Common.Constants;
using Application.Common.Dtos.Request;
using Application.Common.Dtos.Response;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace Infrastructure.Services
{
    public class ThetaVideoService : IThetaVideoService
    {
        private readonly IRestClient _client;
        public ThetaVideoService(IConfiguration configuration)
        {
            var apiEndpoint = configuration.GetValue<string>(EnvironmentVariableKeys.THETAVIDEOAPIENDPOINT);
            var apiKey = configuration.GetValue<string>(EnvironmentVariableKeys.THETAVIDEOAPIKEY);
            var apiSecret = configuration.GetValue<string>(EnvironmentVariableKeys.THETAVIDEOAPISECRET);

            _client = new RestClient(apiEndpoint);
            _client.AddDefaultHeader("x-tva-sa-id", apiKey);
            _client.AddDefaultHeader("x-tva-sa-secret", apiSecret);
        }

        private ThetaVideoProgressResultDto Execute(RestRequest request)
        {
            var response = _client.Execute<ThetaVideoProgressResultDto>(request);

            if(response == null || response.Data == null)
            {

            }

            return response.Data;
        }

        private void ProcessResponse(IRestResponse<ThetaVideoProgressResultDto> response)
        {
            if (!response.IsSuccessful)
            {
                throw new ThetaVideoApiServerErrorException("There's a problem on Theta Video API Service. Please try again later.");
            }
        }

        public ThetaVideoProgressResultDto Transcode(string sourceUri)
        {
            var request = new RestRequest("video", Method.POST, DataFormat.Json);
            var json = new ThetaVideoTranscodeRequestDto()
            {
                SourceUri = sourceUri,
                PlaybackPolicy = "public"
            };

            var s = JsonConvert.SerializeObject(json);
            request.AddParameter("application/json", s, ParameterType.RequestBody);

            var response = Execute(request);
            return response;
        }

        public ThetaVideoProgressResultDto GetVideoProgress(string id)
        {
            var request = new RestRequest($"video/{id}", Method.GET);
            var response = Execute(request);
            return response;
        }
    }
}
