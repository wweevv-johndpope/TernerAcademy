using Newtonsoft.Json;

namespace Application.Common.Dtos.Request
{
    public class ThetaVideoTranscodeRequestDto
    {
        [JsonProperty("source_uri")]
        public string SourceUri { get; set; }

        [JsonProperty("playback_policy")]
        public string PlaybackPolicy { get; set; } = "playback_policy";
    }
}
