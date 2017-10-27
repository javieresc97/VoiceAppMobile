using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using VoicePay.Services.Interfaces;
using VoicePay.ViewModel;
using Xamarin.Forms;

namespace VoicePay
{
    public partial class VoicePayPage : ContentPage
    {
        AudioRecorderViewModel viewModel;
        public VoicePayPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new AudioRecorderViewModel();
        }

        protected override async void OnAppearing()
        {
            await viewModel.CheckPermissions();
            if (viewModel.PermissionStatus == PermissionStatus.Granted)
            {
                RecordBtn.Pressed += RecordBtn_Pressed;
                RecordBtn.Released += RecordBtn_Released;
            }
        }

        void RecordBtn_Pressed(object sender, System.EventArgs e)
        {
            viewModel.StartRecordingCommand.Execute(null);
        }

        void RecordBtn_Released(object sender, System.EventArgs e)
        {
            viewModel.StopRecordingCommand.Execute(null);
        }
    }
}
