using System.Globalization;

namespace CS4N.EnergyHistory.MobileApp.Converters
{
  /// <summary>
  /// Kehrt einen bool-Wert um. Wird für IsVisible-Bindings benötigt, wenn z.B.
  /// ein Element sichtbar sein soll solange IsBusy false ist.
  /// Entspricht dem sap.ui.core.format.NumberFormat-Muster in UI5, nur umgekehrt.
  /// </summary>
  public class InvertBoolConverter : IValueConverter
  {
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
      => value is bool b ? !b : false;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
      => value is bool b ? !b : false;
  }

  /// <summary>
  /// Liefert true wenn ein String nicht leer ist.
  /// Nützlich für IsVisible-Bindings an Text-Properties wie FooterText oder ErrorMessage.
  /// </summary>
  public class StringNotEmptyConverter : IValueConverter
  {
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
      => value is string s && !string.IsNullOrEmpty(s);

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
      => throw new NotImplementedException();
  }

  /// <summary>
  /// Liefert true wenn ein Objekt nicht null ist. 
  /// Wird für IsVisible-Bindings an nullable Properties wie Definition verwendet.
  /// </summary>
  public class NotNullConverter : IValueConverter
  {
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
      => value != null;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
      => throw new NotImplementedException();
  }
}
