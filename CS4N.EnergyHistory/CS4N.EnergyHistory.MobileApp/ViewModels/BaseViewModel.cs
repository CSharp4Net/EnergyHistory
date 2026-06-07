using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CS4N.EnergyHistory.MobileApp.ViewModels
{
  /// <summary>
  /// Basisklasse für alle ViewModels. Implementiert INotifyPropertyChanged,
  /// damit die MAUI-Datenbindung bei Eigenschaftsänderungen die UI aktualisiert.
  /// 
  /// Das MVVM-Muster trennt UI-Logik (ViewModel) von der Darstellung (View),
  /// analog zu den UI5-Controllern im WebApp-Projekt, nur typsicher und
  /// direkt in C# statt JavaScript.
  /// </summary>
  public abstract class BaseViewModel : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler? PropertyChanged;

    // CallerMemberName ermittelt den Namen der aufrufenden Eigenschaft
    // automatisch zur Compile-Zeit, sodass wir keine "magic strings" brauchen.
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
      => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    /// <summary>
    /// Setzt einen Wert nur, wenn er sich geändert hat, und löst
    /// PropertyChanged aus. Gibt zurück, ob eine Änderung stattfand.
    /// </summary>
    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
      if (EqualityComparer<T>.Default.Equals(field, value))
        return false;

      field = value;
      OnPropertyChanged(name);
      return true;
    }

    // --- Allgemeine Zustandseigenschaften ---

    private bool _isBusy;
    public bool IsBusy
    {
      get => _isBusy;
      set => SetProperty(ref _isBusy, value);
    }

    private string _errorMessage = string.Empty;
    public string ErrorMessage
    {
      get => _errorMessage;
      set
      {
        SetProperty(ref _errorMessage, value);
        OnPropertyChanged(nameof(HasError));
      }
    }

    public bool HasError => !string.IsNullOrEmpty(_errorMessage);

    /// <summary>
    /// Führt eine asynchrone Aktion mit automatischer Busy-Anzeige und
    /// Fehlerbehandlung aus. Das Muster entspricht dem try/finally-Block in
    /// den UI5-Controllern mit busy/unbusy.
    /// </summary>
    protected async Task RunWithBusyAsync(Func<Task> action)
    {
      IsBusy = true;
      ErrorMessage = string.Empty;
      try
      {
        await action();
      }
      catch (Exception ex)
      {
        ErrorMessage = ex.Message;
      }
      finally
      {
        IsBusy = false;
      }
    }
  }
}
