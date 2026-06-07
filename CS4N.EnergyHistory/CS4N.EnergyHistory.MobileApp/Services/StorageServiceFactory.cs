namespace CS4N.EnergyHistory.MobileApp.Services
{
  /// <summary>
  /// Fabrik für den Speicherdienst. Liest die Einstellungen und entscheidet,
  /// ob lokal oder remote gespeichert werden soll.
  /// Alle Einstellungsschlüssel sind hier zentralisiert, damit sie nicht
  /// über mehrere Klassen verstreut sind.
  /// </summary>
  public static class StorageServiceFactory
  {
    public const string KeyStorageMode = "storage_mode";
    public const string KeyRemoteUrl = "storage_remote_url";

    public const string ModeLocal = "local";
    public const string ModeRemote = "remote";

    /// <summary>
    /// Erstellt den passenden Speicherdienst anhand der aktuellen Einstellungen.
    /// </summary>
    public static IStorageService Create()
    {
      string mode = Preferences.Get(KeyStorageMode, ModeLocal);

      if (mode == ModeRemote)
      {
        string url = Preferences.Get(KeyRemoteUrl, string.Empty);
        if (!string.IsNullOrWhiteSpace(url))
          return new RemoteStorageService(url);
      }

      // Fallback: lokale Speicherung
      return new LocalStorageService();
    }
  }
}
