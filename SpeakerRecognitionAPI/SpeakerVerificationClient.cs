using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpeakerRecognitionAPI.Models;

namespace SpeakerRecognitionAPI
{
    public class SpeakerVerificationClient : SpeakerServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SpeakerRecognitionAPI.SpeakerVerificationClient"/> class.
        /// </summary>
        /// <param name="subscriptionKey">Subscription key for the Speaker Recognition API.</param>
        protected SpeakerVerificationClient(string subscriptionKey) : base(subscriptionKey)
        {
        }

        public async Task<VerificationResponse> VerifyAsync(string audioFilePath, string profileId)
        {
            try
            {
                var requestUri = string.Format(Constants.Endpoints.SpeakerVerify.ToString(), profileId);
                var request = PrepareMediaRequest(audioFilePath, requestUri);

                var response = await _httpClient.SendAsync(request);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    throw BuildErrorFromServiceResult(jsonResponse);

                var result = JsonConvert.DeserializeObject<VerificationResponse>(jsonResponse);
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("EXCEPTION: " + ex);
                throw;
            }
        }
    }
}
