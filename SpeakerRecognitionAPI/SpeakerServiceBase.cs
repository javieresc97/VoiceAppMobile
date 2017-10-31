using System;
using System.Net.Http;
using Newtonsoft.Json;
using SpeakerRecognitionAPI.Helpers;
using SpeakerRecognitionAPI.Models;

namespace SpeakerRecognitionAPI
{
    public abstract class SpeakerServiceBase
    {
        protected readonly HttpClient _httpClient;
        protected readonly string _subscriptionKey;

        protected SpeakerServiceBase(string subscriptionKey)
        {
            _subscriptionKey = subscriptionKey;
            _httpClient = new HttpClient();
        }

        protected SpeakerRecognitionException BuildErrorFromServiceResult(string result, string errorMessage = "Error sending request")
        {
            var errorResponse = JsonConvert.DeserializeObject<ServiceError>(result);
            var ex = new SpeakerRecognitionException(errorMessage)
            {
                DetailedError = errorResponse.Error
            };
            return ex;
        }

        protected HttpRequestMessage PrepareMediaRequest(string audioFilePath, string requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Headers.TransferEncodingChunked = true;
            request.Headers.ExpectContinue = true;
            request.Headers.Accept.ParseAdd(Constants.MimeTypes.Json);
            request.Headers.Accept.ParseAdd(Constants.MimeTypes.Xml);
            request.Content = MediaRequestHelper.PopulateRequestContent(audioFilePath);
            return request;
        }
    }
}
