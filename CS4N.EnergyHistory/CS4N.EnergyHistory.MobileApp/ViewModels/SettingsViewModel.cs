using CS4N.EnergyHistory.MobileApp.Services;

namespace CS4N.EnergyHistory.MobileApp.ViewModels
{
  /// <summary>
  /// ViewModel für die Einstellungsseite.
  /// Liest und schreibt die Benutzerpräferenzen mit Maui.Essentials.Preferences,
  /// welches plattformübergreifend sicher persistiert (SharedPreferences auf Android,
  /// NSUserDefaults auf iOS, Registry auf Windows).
  /// </summary>
  public class SettingsViewModel : BaseViewModel
  {
    // Diese Eigenschaft steuert, welche Eingabefelder sichtbar sind
    private bool _isRemoteMode;
    public bool IsRemoteMode
    {
      get => _isRemoteMode;
      set
      {
        SetProperty(ref _isRemoteMode, value);
        // Abgeleitete Visible-Property ebenfalls notifizieren
        OnPropertyChanged(nameof(IsLocalMode));
      }
    }

    // Für den RadioButton "Lokal" — ist die Negierung von IsRemoteMode
    public bool IsLocalMode
    {
      get => !_isRemoteMode;
      set => IsRemoteMode = !value;
    }

    private string _remoteUrl = string.Empty;
    public string RemoteUrl
    {
      get => _remoteUrl;
      set => SetProperty(ref _remoteUrl, value);
    }

    // Statusmeldung nach dem Speichern
    private string _savedMessage = string.Empty;
    public string SavedMessage
    {
      get => _savedMessage;
      set { SetProperty(ref _savedMessage, value); OnPropertyChanged(nameof(HasSavedMessage)); }
    }
    public bool HasSavedMessage => !string.IsNullOrEmpty(_savedMessage);

    public SettingsViewModel()
    {
      // Gespeicherte Einstellungen laden
      var mode = Preferences.Get(StorageServiceFactory.KeyStorageMode, StorageServiceFactory.ModeLocal);
      _isRemoteMode = mode == StorageServiceFactory.ModeRemote;
      _remoteUrl = Preferences.Get(StorageServiceFactory.KeyRemoteUrl, string.Empty);
    }

    public void Save()
    {
      Preferences.Set(
        StorageServiceFactory.KeyStorageMode,
        _isRemoteMode ? StorageServiceFactory.ModeRemote : StorageServiceFactory.ModeLocal);

      Preferences.Set(StorageServiceFactory.KeyRemoteUrl, _remoteUrl);

      SavedMessage = "Einstellungen gespeichert.";

      // Meldung nach 3 Sekunden wieder ausblenden
      Task.Delay(3000).ContinueWith(_ =>
        MainThread.BeginInvokeOnMainThread(() => SavedMessage = string.Empty));
    }
  }
}
