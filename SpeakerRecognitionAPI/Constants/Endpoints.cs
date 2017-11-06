using System;
namespace SpeakerRecognitionAPI.Constants
{
    internal static class Endpoints
    {
        #region Identification

        internal static WestUsSpeakerRecognitionEndpoint IdentificationCreateProfile
                = new WestUsSpeakerRecognitionEndpoint("/identificationProfiles");

        internal static WestUsSpeakerRecognitionEndpoint IdentificationEnroll 
                = new WestUsSpeakerRecognitionEndpoint("/identificationProfiles/{0}/enroll");

        internal static WestUsSpeakerRecognitionEndpoint Identify
                = new WestUsSpeakerRecognitionEndpoint("/identify?identificationProfileIds={0}");

        #endregion


        #region Verification

        internal static WestUsSpeakerRecognitionEndpoint VerificationCreateProfile
                = new WestUsSpeakerRecognitionEndpoint("/verificationProfiles");

        internal static WestUsSpeakerRecognitionEndpoint VerificationEnroll
                = new WestUsSpeakerRecognitionEndpoint("/verificationProfiles/{0}/enroll");

        internal static WestUsSpeakerRecognitionEndpoint Verify
                = new WestUsSpeakerRecognitionEndpoint("/verify?verificationProfileId={0}");

        internal static WestUsSpeakerRecognitionEndpoint VerificationPhrases
                = new WestUsSpeakerRecognitionEndpoint("/verificationPhrases?locale={0}");

        #endregion


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
                return $"{BaseServiceUri}{_relativePath}";
            }
        }
    }

}
