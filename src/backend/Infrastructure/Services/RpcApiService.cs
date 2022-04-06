using Application.Common.Constants;
using Application.Common.Dtos.Request;
using Application.Common.Dtos.Response;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace Infrastructure.Services
{
    public class RpcApiService : IRpcApiService
    {
        private readonly IRestClient _client;
        public RpcApiService(IConfiguration configuration)
        {
            var baseUrl = configuration.GetValue<string>(EnvironmentVariableKeys.RPCENDPOINT);
            _client = new RestClient(baseUrl);
        }

        private void Execute(RestRequest request)
        {
            var response = _client.Execute(request);
            ProcessResponse(response);
        }

        private T Execute<T>(RestRequest request)
        {
            var response = _client.Execute<T>(request);
            ProcessResponse(response);
            return response.Data;
        }

        private int GetId() => new Random().Next(0, int.MaxValue);

        private void ProcessResponse(IRestResponse response)
        {
            if (!response.IsSuccessful)
            {
                throw new RpcServerErrorException("There's a problem on ETH Adapter RPC JSON-RPC Service. Please try again.");
            }
        }

        public RpcApiResultDto<string> GetTransactionCount(string address)
        {
            var request = new RestRequest("", Method.POST, DataFormat.Json);
            var json = new RpcApiRequestDto("eth_getTransactionCount", address, "latest");
            json.Id = GetId();

            var s = JsonConvert.SerializeObject(json);
            request.AddParameter("application/json", s, ParameterType.RequestBody);

            return Execute<RpcApiResultDto<string>>(request);
        }

        public RpcApiResultDto<RpcApiTransactionResultDto> GetTransactionByHash(string txHash)
        {
            var request = new RestRequest("", Method.POST, DataFormat.Json);
            var json = new RpcApiRequestDto("eth_getTransactionByHash", txHash);
            json.Id = GetId();

            var s = JsonConvert.SerializeObject(json);
            request.AddParameter("application/json", s, ParameterType.RequestBody);

            return Execute<RpcApiResultDto<RpcApiTransactionResultDto>>(request);
        }

        public RpcApiResultDto<string> GetBalance(string address)
        {
            var request = new RestRequest("", Method.POST, DataFormat.Json);
            var json = new RpcApiRequestDto("eth_getBalance", address, "latest");
            json.Id = GetId();

            var s = JsonConvert.SerializeObject(json);
            request.AddParameter("application/json", s, ParameterType.RequestBody);

            return Execute<RpcApiResultDto<string>>(request);
        }

        public RpcApiResultDto<string> SendPayment(string from, string to, string valueInHex)
        {
            var request = new RestRequest("", Method.POST, DataFormat.Json);
            var json = new RpcApiRequestDto("eth_sendTransaction", new Dictionary<string, object>
            {
                { "from", from },
                { "to", to },
                { "gas", "0x76c0" },
                { "gasPrice", "0x9184e72a000" },
                { "value", valueInHex },
                { "data", "0xd46e8dd67c5d32be8d46e8dd67c5d32be8058bb8eb970870f072445675058bb8eb970870f072445675" },
            });

            json.Id = GetId();

            var s = JsonConvert.SerializeObject(json);
            request.AddParameter("application/json", s, ParameterType.RequestBody);

            return Execute<RpcApiResultDto<string>>(request);
        }
    }
}
