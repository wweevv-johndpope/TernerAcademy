using Newtonsoft.Json;

namespace Application.Common.Dtos.Response
{
    public class RpcApiErrorResultDto
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
