using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Application.Common.Dtos.Request
{
    public class RpcApiRequestDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("jsonrpc")]
        public string JsonRpc { get; private set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("params")]
        public List<object> Params { get; set; }

        public RpcApiRequestDto(string method, params object[] payload)
        {
            JsonRpc = "2.0";
            Method = method;
            Params = payload.ToList();
        }
    }
}