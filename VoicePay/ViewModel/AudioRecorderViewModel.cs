using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using VoicePay.Services.Interfaces;
using Xamarin.Forms;

namespace VoicePay.ViewModel
{
    public class AudioRecorderViewModel : BaseViewModel
    {
        private readonly IAudioRecorder _recorder;

        public ICommand StartRecordingCommand { get; private set; }
        public ICommand StopRecordingCommand { get; private set; }
        public PermissionStatus PermissionStatus { get; private set; }

        public AudioRecorderViewModel()
        {
            _recorder = DependencyService.Get<IAudioRecorder>();

            StartRecordingCommand = new Command(StartRecording);
            StopRecordingCommand = new Command(StopRecording);
        }


        public async Task CheckPermissions()
        {
            PermissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Microphone);
            if (PermissionStatus != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Microphone))
                {
                    await DisplayAlert("Necesitamos tu permiso", "Por favor, permítenos el uso de tu micrófono.", "OK");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Microphone);
                if (results.ContainsKey(Permission.Microphone))
                    PermissionStatus = results[Permission.Microphone];
            }

            if (PermissionStatus == PermissionStatus.Granted)
            {
                _recorder.SetUp();
            }
            else
            {
                await DisplayAlert("Lo sentimos", "No podremos grabar tu audio", "OK");
            }
        }

        private void StartRecording(object obj)
        {
            _recorder.StartRecording();
        }

        private void StopRecording(object obj)
        {
            _recorder.StopRecording();
        }

    }
}
