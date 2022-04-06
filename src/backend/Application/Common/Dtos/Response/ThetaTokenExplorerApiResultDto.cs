using Newtonsoft.Json;

namespace Application.Common.Dtos.Response
{
    public class ThetaTokenExplorerApiResultDto<T>
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("body")]
        public T Body { get; set; }
    }
}
