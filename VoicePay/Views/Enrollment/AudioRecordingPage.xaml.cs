using System;
using System.Collections.Generic;
using VoicePay.ViewModels.Enrollment;
using Xamarin.Forms;

namespace VoicePay.Views.Enrollment
{
    public partial class AudioRecordingPage : ContentPage
    {
        AudioRecordingViewModel viewModel;

        public AudioRecordingPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new AudioRecordingViewModel();
        }

        protected override async void OnAppearing()
        {
            await viewModel.StartRecording();
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }
    }
}
