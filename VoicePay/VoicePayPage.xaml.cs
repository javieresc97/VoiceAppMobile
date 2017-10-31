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
                RecordBtn.Clicked += RecordBtn_Pressed;
                SendBtn.Clicked += SendBtn_Clicked;
            }
        }

        void RecordBtn_Pressed(object sender, System.EventArgs e)
        {
            if(!viewModel.IsRecordingAudio)
            {
                viewModel.StartRecordingCommand.Execute(null);
                RecordBtn.Text = "Stop";
            }
            else
            {
                viewModel.StopRecordingCommand.Execute(null);
                RecordBtn.Text = "Grabar";
            }

        }

        async void SendBtn_Clicked(object sender, System.EventArgs e)
        {
            await viewModel.CreateProfile();
        }
    }
}
