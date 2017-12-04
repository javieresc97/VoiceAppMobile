
using Xamarin.Forms;
using VoicePay.ViewModels;
using VoicePay.Helpers;

namespace VoicePay.Views.Enrollment
{
    public partial class CorrectResultPage : ContentPage
    {
        public CorrectResultPage()
        {
            InitializeComponent();
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await BaseViewModel.MasterDetail.Detail.Navigation.PopToRootAsync();
            await BaseViewModel.MasterDetail.Detail.Navigation.PopModalAsync();
            Cart.Instance.Clear();
        }
    }
}
