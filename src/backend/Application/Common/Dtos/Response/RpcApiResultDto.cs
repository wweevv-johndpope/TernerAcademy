using Newtonsoft.Json;

namespace Application.Common.Dtos.Response
{
    public class RpcApiResultDto<T>
    {
        [JsonProperty("error")]
        public RpcApiErrorResultDto Error { get; set; }

        [JsonProperty("result")]
        public T Result { get; set; }
    }
}
