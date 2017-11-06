using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Permissions.Abstractions;
using VoicePay.Views;
using Xamarin.Forms;
using Plugin.Permissions;
using SpeakerRecognitionAPI;
using VoicePay.Constants;
using VoicePay.Helpers;
using System.Diagnostics;
using VoicePay.Services;
using VoicePay.Views.Enrollment;

namespace VoicePay.ViewModels.Enrollment
{
    public class WelcomeViewModel : BaseViewModel
    {
        private readonly SpeakerVerificationClient _client;
        private readonly IPermissions _permissionService;

        public PermissionStatus PermissionStatus { get; private set; }
        public ICommand CheckAndGoCommand { get; private set; }
        public ICommand GoBackCommand { get; private set; }

        private bool IsProfileCreated 
        { 
            get 
            { 
                return Settings.Instance.Contains(nameof(Settings.UserIdentificationId)); 
            } 
        }


        public WelcomeViewModel() : this(CrossPermissions.Current) { }
        public WelcomeViewModel(IPermissions permissionService)
        {
            _permissionService = permissionService;
            _client = VerificationService.Instance;

            CheckAndGoCommand = new Command(async () => await CheckPermissions());
            GoBackCommand = new Command(async () => await GoBack());
        }


        #region Command actions

        private async Task CheckPermissions()
        {
            IsBusy = true;

            await RequestPermissionsIfNotGranted();

            if (PermissionStatus == PermissionStatus.Granted)
            {
                if (!IsProfileCreated)
                {
                    await TryCreateProfile();
                }

                if (IsProfileCreated)
                {
                    await GoToEnrollmentProcess();
                    IsBusy = false;
                }
                else
                {
                    IsBusy = false;
                    await DisplayAlert("¡Ups!", "Ocurrió un error inesperado. Intente nuevamente más tarde.", "OK");
                }
            }
            else
            {
                IsBusy = false;
                await DisplayAlert("¡Ups!", "No podemos continuar si no nos permiso para acceder a tu micrófono.", "OK");
            }

        }


        private async Task GoBack()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        #endregion


        private async Task RequestPermissionsIfNotGranted()
        {
            PermissionStatus = await _permissionService.CheckPermissionStatusAsync(Permission.Microphone);
            if (PermissionStatus != PermissionStatus.Granted)
            {
                if (await _permissionService.ShouldShowRequestPermissionRationaleAsync(Permission.Microphone))
                {
                    await DisplayAlert("Permisos", "Debes autorizar el uso de tu micrófono para continuar.", "OK");
                }

                var results = await _permissionService.RequestPermissionsAsync(Permission.Microphone);
                if (results.ContainsKey(Permission.Microphone))
                    PermissionStatus = results[Permission.Microphone];
            }
        }

        private async Task GoToEnrollmentProcess()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SelectPhrasePage());
        }


        private async Task TryCreateProfile()
        {
            try
            {
                var profile = await _client.CreateProfileAsync();
                if (!string.IsNullOrEmpty(profile.VerificationProfileId))
                {
                    Settings.UserIdentificationId = profile.VerificationProfileId;
                }
            }
            catch
            {
                Debug.WriteLine("Error trying to create profile.");
            }
        }

    }
}
