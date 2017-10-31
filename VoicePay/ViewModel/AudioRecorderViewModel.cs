using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.AudioRecorder;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SpeakerRecognitionAPI.API;
using SpeakerRecognitionAPI.Helpers;
using SpeakerRecognitionAPI.Models;
using VoicePay.Services.Interfaces;
using Xamarin.Forms;

namespace VoicePay.ViewModel
{
    public class AudioRecorderViewModel : BaseViewModel
    {
        private readonly AudioRecorderService _recorder;
        private readonly SpeakerIdentificationClient _speakerClient;
        private readonly IPermissions _permissionService;

        public ICommand StartRecordingCommand { get; private set; }
        public ICommand StopRecordingCommand { get; private set; }
        public PermissionStatus PermissionStatus { get; private set; }
        public bool IsRecordingAudio => _recorder.IsRecording;

        //  TODO: Grab it from Settings
        string identificationProfileId = "fad371a6-fbec-4e06-abe8-5df9cfb0f497";


        public AudioRecorderViewModel() : this (CrossPermissions.Current){ }
        public AudioRecorderViewModel(IPermissions permissionService)
        {
            _permissionService = permissionService;
            _speakerClient = new SpeakerIdentificationClient(Constants.Keys.SpeakerRecognitionApiKey);
            _recorder = new AudioRecorderService
            {
                StopRecordingAfterTimeout = false,
                StopRecordingOnSilence = false,
                PreferredSampleRate = 16000 //  Required for the API
            };

            StartRecordingCommand = new Command(async () => await StartRecording());
            StopRecordingCommand = new Command(async () => await StopRecording());
        }


        public async Task CheckPermissions()
        {
            PermissionStatus = await _permissionService.CheckPermissionStatusAsync(Permission.Microphone);
            if (PermissionStatus != PermissionStatus.Granted)
            {
                if (await _permissionService.ShouldShowRequestPermissionRationaleAsync(Permission.Microphone))
                {
                    await DisplayAlert("Un momento", "Debes autorizar el uso de tu micrófono para continuar.", "OK");
                }

                var results = await _permissionService.RequestPermissionsAsync(Permission.Microphone);
                if (results.ContainsKey(Permission.Microphone))
                    PermissionStatus = results[Permission.Microphone];
            }

            if (PermissionStatus == PermissionStatus.Granted)
            {
                _recorder.AudioInputReceived += Recorder_AudioInputReceived;
            }
            else
            {
                await DisplayAlert("Lo sentimos", "No podremos grabar tu audio", "OK");
            }
        }

        private async Task StartRecording()
        {
            if (PermissionStatus == PermissionStatus.Granted)
            {
                await _recorder.StartRecording();
            }
        }

        private async Task StopRecording()
        {
            if (PermissionStatus == PermissionStatus.Granted)
            {
                await _recorder.StopRecording();
            }
        }

        private async void Recorder_AudioInputReceived(object sender, string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                await DisplayAlert("Grabado", " -- " + filePath, "OK");

                try
                {
                    //var trackingUrl = await _speakerClient.EnrollAsync(filePath, identificationProfileId);
                    //var response = await _speakerClient.CheckEnrollmentStatus(trackingUrl);
                    ////var output = $"{result.EnrollmentStatus}. " +
                    ////$"Dijiste {result.Phrase}" +
                    ////$"van {result.EnrollmentsCount} " +
                    ////$"y faltan {result.RemainingEnrollments} audios más.";
                    //await DisplayAlert("Exito", ":D", "OK");
                    //var verification = await _speakerClient.VerifyAsync(filePath, identificationProfileId);
                    //await DisplayAlert("Resultado", $"{verification.Result}: {verification.Confidence} - {verification.Phrase}", "OK");
                }
                catch (SpeakerRecognitionException ex)
                {
                    await DisplayAlert("Ups", $"{ex.DetailedError.Code}: {ex.DetailedError.Message}", "OK");
                }
            }
            else
            {
                await DisplayAlert("Ups", "No se ha grabado nada, intenta nuevamente.", "OK");
            }
        }

        public async Task CreateProfile()
        {
            var response = await _speakerClient.CreateProfileAsync();
            identificationProfileId = response.IdentificationProfileId;
            await DisplayAlert("Perfil creado", identificationProfileId, "OK");
        }
    }
}
