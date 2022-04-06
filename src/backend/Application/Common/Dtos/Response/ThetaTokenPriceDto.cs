using Newtonsoft.Json;
using System;

namespace Application.Common.Dtos.Response
{
    public class ThetaTokenPriceDto
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("circulating_supply")]
        public long CirculatingSupply { get; set; }

        [JsonProperty("last_updated")]
        public DateTimeOffset LastUpdated { get; set; }

        [JsonProperty("market_cap")]
        public double MarketCap { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("total_supply")]
        public long TotalSupply { get; set; }

        [JsonProperty("volume_24h")]
        public double Volume24H { get; set; }
    }
}
