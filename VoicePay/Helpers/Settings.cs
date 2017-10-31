// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace VoicePay.Helpers
{
    public static class Settings
	{
        static IVoiceAppSettings settings;
        public static IVoiceAppSettings Instance
        {
            get
            {
                return settings ?? (settings = new VoiceAppSettings());
            }
            set
            {
                settings = value;
            }
        }
	}

    public interface IVoiceAppSettings
    {
        string UserIdentificationId { get; set; }
    }

    public class VoiceAppSettings : IVoiceAppSettings
    {
        private ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants

        private const string UserIdentificationIdKey = "user_identification_id";
        private static readonly string UserIdentificationIdDefault = string.Empty;

        #endregion

        public string UserIdentificationId 
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserIdentificationIdKey, UserIdentificationIdDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserIdentificationIdKey, value);
            }
        }
    }
}