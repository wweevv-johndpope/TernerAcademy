using Application.Common.JsonHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Application.Common.Dtos.Response
{
    public class ThetaVideoProgressResultDto
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("body")]
        public Body Body { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public partial class Body
    {
        [JsonProperty("videos")]
        public List<Video> Videos { get; set; }
    }

    public partial class Video
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("playback_uri")]
        public string PlaybackUri { get; set; }

        [JsonProperty("player_uri")]
        public string PlayerUri { get; set; }

        [JsonProperty("create_time")]
        public DateTimeOffset CreateTime { get; set; }

        [JsonProperty("update_time")]
        public DateTimeOffset UpdateTime { get; set; }

        [JsonProperty("service_account_id")]
        public string ServiceAccountId { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("sub_state")]
        public string SubState { get; set; }

        [JsonProperty("source_upload_id")]
        public object SourceUploadId { get; set; }

        [JsonProperty("source_uri")]
        public string SourceUri { get; set; }

        [JsonProperty("playback_policy")]
        public string PlaybackPolicy { get; set; }

        [JsonProperty("progress")]
        public long Progress { get; set; }

        [JsonProperty("error")]
        public object Error { get; set; }

        [JsonProperty("duration")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public string Duration { get; set; }

        [JsonProperty("resolution")]
        public long Resolution { get; set; }
    }

}
