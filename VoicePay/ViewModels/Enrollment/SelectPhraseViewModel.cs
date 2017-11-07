using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using SpeakerRecognitionAPI.Models;
using VoicePay.Services;
using Xamarin.Forms;
using MvvmHelpers;
using FormsToolkit;

namespace VoicePay.ViewModels.Enrollment
{
    public class SelectPhraseViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Phrase> Phrases { get; set; }

        public SelectPhraseViewModel()
        {
            IsBusy = true;
            Phrases = new ObservableRangeCollection<Phrase>();
        }

        public async Task GetPhrases()
        {
            IsBusy = true;

            try
            {
                var phrases = await VerificationService.Instance.GetPhrasesAsync();
                if (phrases.Any())
                {
                    Phrases.ReplaceRange(phrases);
                }
                else
                {
                    await DisplayAlert("¡Ups!", "El servicio no está disponible ahora. Inténtalo nuevamente.", "OK");
                }
            }
            catch
            {
                await DisplayAlert("¡Ups!", "El servicio no está disponible ahora. Inténtalo nuevamente.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
