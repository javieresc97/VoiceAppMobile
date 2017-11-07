using VoicePay.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace VoicePay
{
    
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //Settings.Instance.Clear();
            MainPage = new NavigationPage(new Views.Enrollment.WelcomePage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
