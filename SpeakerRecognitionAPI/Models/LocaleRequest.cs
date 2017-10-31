using System;
using Newtonsoft.Json;

namespace SpeakerRecognitionAPI.Models
{
    public class LocaleRequest
    {
        [JsonProperty("locale")]
        public string Locale { get; set; } = "en-us";
    }
}
