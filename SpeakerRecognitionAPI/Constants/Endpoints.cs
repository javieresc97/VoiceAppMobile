using System;
namespace SpeakerRecognitionAPI.Constants
{
    internal static class Endpoints
    {
        internal static WestUsSpeakerRecognitionEndpoint SpeakerIdentificationEnroll 
                = new WestUsSpeakerRecognitionEndpoint("/identificationProfiles/{0}/enroll");

        internal static WestUsSpeakerRecognitionEndpoint SpeakerIdentificationCreateProfile
                = new WestUsSpeakerRecognitionEndpoint("/identificationProfiles");

        internal static WestUsSpeakerRecognitionEndpoint SpeakerVerify
                = new WestUsSpeakerRecognitionEndpoint("/verify?verificationProfileId={0}");

        internal static WestUsSpeakerRecognitionEndpoint SpeakerIdentify
                = new WestUsSpeakerRecognitionEndpoint("/identify?identificationProfileIds={0}");


        internal class WestUsSpeakerRecognitionEndpoint
        {
            private const string BaseServiceUri = "https://westus.api.cognitive.microsoft.com/spid/v1.0";
            private string _relativePath;

            public WestUsSpeakerRecognitionEndpoint(string relativePath)
            {
                _relativePath = relativePath;
            }

            public override string ToString()
            {
                return string.Format($"{BaseServiceUri}{_relativePath}");
            }
        }
    }

}
