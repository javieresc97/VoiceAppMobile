using Newtonsoft.Json;
namespace SpeakerRecognitionAPI.Models
{
    public class ProfileResponse
    {
        [JsonProperty("identificationProfileId")]
        public string IdentificationProfileId { get; set; }
    }
}
