using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.AudioRecorder;
using SpeakerRecognitionAPI.Helpers;
using SpeakerRecognitionAPI.Models;
using VoicePay.Helpers;
using VoicePay.Services;
using VoicePay.Services.Interfaces;
using Xamarin.Forms;

namespace VoicePay.ViewModels.Enrollment
{
    public class AudioRecordingViewModel : BaseViewModel
    {
        private readonly AudioRecorderService _recorder;
        private readonly IBeepPlayer _beeper;
        private string _phraseMessage => $"\"{EnrollmentProcess.SelectedPhrase.Text}\"";

        public EnrollmentVerification Enrollment { get; set; } = new EnrollmentVerification();

        private string _stateMessage;
        public string StateMessage
        {
            get { return _stateMessage; }
            set { _stateMessage = value; RaisePropertyChanged(); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; RaisePropertyChanged(); }
        }


        public AudioRecordingViewModel()
        {
            StateMessage = "Espera...";

            _beeper = DependencyService.Get<IBeepPlayer>();
            _recorder = new AudioRecorderService
            {
                AudioSilenceTimeout = TimeSpan.FromSeconds(4),
                StopRecordingOnSilence = true,
                StopRecordingAfterTimeout = false,
                PreferredSampleRate = 16000,
                SilenceThreshold = 0.1f
            };
            _recorder.AudioInputReceived += Recorder_AudioInputReceived;
        }


        private async void Recorder_AudioInputReceived(object sender, string audioFilePath)
        {
            if (string.IsNullOrEmpty(audioFilePath))
            {
                StateMessage = "No logramos escucharte :/";
                Message = "Intenta hablando mas fuerte";
                await WaitAndStartRecording();
                return;
            }

            IsBusy = true;

            StateMessage = "Analizando audio...";
            Message = string.Empty;

            try
            {
                var enrollmentResult = await VerificationService.Instance.EnrollAsync(audioFilePath, Settings.UserIdentificationId);
                EnrollmentProcess.SelectedPhrase.Text = enrollmentResult.Phrase;

                if (Enrollment.RemainingEnrollments > 0)
                {
                    await StartRecording();
                }
                else
                {
                    StateMessage = "¡Muy bien! Hemos terminado.";
                }
            }
            catch (SpeakerRecognitionException ex)
            {
                if (ex.DetailedError.Message.Equals("InvalidPhrase", StringComparison.OrdinalIgnoreCase))
                {
                    StateMessage = "¡Ups! Parece que dijiste una frase no válida";
                    Message = "Intentémoslo denuevo...";
                    await WaitAndStartRecording();
                }
            }
            catch
            {
                StateMessage = "¡Ups! Algo extraño sucedió";
                Message = "Intentémoslo denuevo...";
                await WaitAndStartRecording();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task WaitAndStartRecording()
        {
            await Task.Delay(3000);
            await StartRecording();
        }

        public async Task StartRecording()
        {
            await _recorder.StartRecording();
            _beeper.Beep();
            StateMessage = "Escuchando...";
            Message = _phraseMessage;
        }

    }
}
