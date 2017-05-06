using System;
using System.Windows.Input;

using RidoShop.Client.Helpers;
using RidoShop.Client.Services;

using Windows.ApplicationModel;

namespace RidoShop.Client.ViewModels
{
    public class SettingsViewModel : Observable
    {
        // TODO UWPTemplates: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings.md
        private bool _isLightThemeEnabled;
        public bool IsLightThemeEnabled
        {
            get { return _isLightThemeEnabled; }
            set { Set(ref _isLightThemeEnabled, value); }
        }

        private string _appDescription;
        public string AppDescription
        {
            get { return _appDescription; }
            set { Set(ref _appDescription, value); }
        }

        public string ServiceUrl => AppConfig.RidoShopServerUrl;

        public ICommand SwitchThemeCommand { get; private set; }

        public SettingsViewModel()
        {
            IsLightThemeEnabled = ThemeSelectorService.IsLightThemeEnabled;
            SwitchThemeCommand = new RelayCommand(async () => { await ThemeSelectorService.SwitchThemeAsync(); });
        }

        public void Initialize()
        {
            AppDescription = GetAppDescription();
        }

        private string GetAppDescription()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{package.DisplayName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
