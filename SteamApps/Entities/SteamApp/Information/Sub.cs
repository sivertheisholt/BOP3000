using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SteamApps.Entities.SteamApp.Information
{
    public class Sub
    {
        public int SubId { get; set; }

        [JsonProperty("packageid")]
        public int PackageId { get; set; }

        [JsonProperty("percent_savings_text")]
        public string PercentSavingsText { get; set; }

        [JsonProperty("percent_savings")]
        public int PercentSavings { get; set; }

        [JsonProperty("option_text")]
        public string OptionText { get; set; }

        [JsonProperty("option_description")]
        public string OptionDescription { get; set; }

        [JsonProperty("can_get_free_license")]
        public string CanGetFreeLicense { get; set; }

        [JsonProperty("is_free_license")]
        public bool IsFreeLicense { get; set; }

        [JsonProperty("price_in_cents_with_discount")]
        public int PriceInCentsWithDiscount { get; set; }

        public PackageGroup PackageGroup { get; set; }
        public int PackageGroupId { get; set; }
    }
}