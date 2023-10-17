using bs.Frmwrk.Core.Dtos.Security;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace bs.Frmwrk.Security.Dtos
{
    public class RecaptchaResponseDto : IRecaptchaResponseDto
    {
        public bool Success { get; set; }

        [JsonPropertyName("challenge_ts")]
        public DateTime CheckDate { get; set; }

        public string Hostname { get; set; }
        public decimal Score { get; set; }
        public string Action { get; set; }

        [JsonProperty(PropertyName = "error-codes")]
        public string[] ErrorCodes { get; set; }
    }
}