using System.Text.Json.Serialization;

namespace GHTK.API.Models
{
    public class ApiResult
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }=string.Empty;
    }
}
