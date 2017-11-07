using System;
using System.Threading.Tasks;
using SpeakerRecognitionAPI.Interfaces;
using SpeakerRecognitionAPI.Models;
using VoicePay.Helpers;
using VoicePay.Services;
using VoicePay.Services.Interfaces;
using Xamarin.Forms;

namespace VoicePay.ViewModels.Enrollment
{
    public class AudioVerifyViewModel : AudioRecordingBaseViewModel
    {
        private readonly IBeepPlayer _beeper;
        private readonly ISpeakerVerification _verificationService;
        private string _phraseMessage => $"\"{Settings.EnrolledPhrase}\"";

        public AudioVerifyViewModel()
        {
            StateMessage = "Espera...";

            _beeper = DependencyService.Get<IBeepPlayer>();
            _verificationService = VerificationService.Instance;

            Recorder.AudioInputReceived += async (object sender, string e) => { await Recorder_AudioInputReceived(sender, e); };
        }

        private async Task Recorder_AudioInputReceived(object sender, string audioFilePath)
        {
            if (string.IsNullOrEmpty(audioFilePath))
            {
                StateMessage = "No logramos escucharte :/";
                Message = "Intenta hablando mas fuerte";
                await WaitAndStartRecording();
                return;
            }

            IsBusy = true;

            StateMessage = "Verificando tu voz...";
            Message = string.Empty;

            try
            {
                var verificationResponse = await _verificationService.VerifyAsync(audioFilePath, Settings.UserIdentificationId);
                if (verificationResponse.Result == Result.Accept)
                {
                    //  Accepted
                    DisplayAlert("BIEN", $"Voz verificada. Confianza: {verificationResponse.Confidence}", "OK");
                }
                else
                {
                    //  Rejected
                    DisplayAlert("NOP", $"Voz no verificada. Confianza {verificationResponse.Confidence}", "OK");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("ERROR", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public override async Task StartRecording()
        {
            await Recorder.StartRecording();
            _beeper.Beep();
            StateMessage = "Escuchando...";
            Message = _phraseMessage;
        }
    }
}
