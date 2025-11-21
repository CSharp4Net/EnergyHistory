using Microsoft.Maui.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace CS4N.EnergyHistory.MobileApp.Controls
{
  // Minimales DTO für Chart-Daten
  public sealed class ChartMonthData
  {
    public string Label { get; set; } = "";
    public double Value { get; set; }
  }

  // Einfacher Drawable für ein Linien-Chart (Achsen, Gitter, Linie, Punkte)
  public class LineChartDrawable : IDrawable
  {
    // Data sollte eine stabile Referenz sein (ObservableCollection)
    public IList<ChartMonthData> Data { get; set; } = new List<ChartMonthData>();

    // Einheit rechts oben
    public string YUnit { get; set; } = "";

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
      canvas.SaveState();

      // Hintergrund
      canvas.FillColor = Colors.White;
      canvas.FillRectangle(dirtyRect);

      // Padding / Plotbereich
      float paddingLeft = 48f, paddingRight = 12f, paddingTop = 12f, paddingBottom = 44f;
      float plotWidth = dirtyRect.Width - paddingLeft - paddingRight;
      float plotHeight = dirtyRect.Height - paddingTop - paddingBottom;

      // Keine Daten
      if (Data == null || Data.Count == 0)
      {
        canvas.FontColor = Colors.Gray;
        canvas.FontSize = 14;
        canvas.DrawString("Keine Daten", dirtyRect, HorizontalAlignment.Center, VerticalAlignment.Center);
        canvas.RestoreState();
        return;
      }

      // Wertebereich
      var values = Data.Select(d => d.Value).ToList();
      double max = Math.Max(1, values.Max());
      double min = 0.0; // startet bei 0

      // Gitterlinien + Y-Achsenbeschriftung
      int ySteps = 4;
      canvas.FontSize = 10;
      for (int i = 0; i <= ySteps; i++)
      {
        float y = paddingTop + (float)(plotHeight - (plotHeight * i / ySteps));
        canvas.StrokeColor = Colors.LightGray;
        canvas.StrokeSize = 1;
        canvas.DrawLine(paddingLeft, y, paddingLeft + plotWidth, y);

        double labelVal = min + (max - min) * i / ySteps;
        canvas.FontColor = Colors.Gray;
        canvas.DrawString(labelVal.ToString("0"), 0, y - 8, paddingLeft, 16, HorizontalAlignment.Center, VerticalAlignment.Top);
      }

      // X-Achsen-Beschriftungen
      int count = Data.Count;
      for (int i = 0; i < count; i++)
      {
        float x = paddingLeft + (float)(plotWidth * i / (count - 1 == 0 ? 1 : count - 1));
        canvas.FontColor = Colors.Gray;
        canvas.FontSize = 10;
        // Label mittig unter dem Tick
        canvas.DrawString(Data[i].Label, x - 22, paddingTop + plotHeight + 6, 44, 18, HorizontalAlignment.Center, VerticalAlignment.Top);
      }

      // Linie zeichnen
      var points = new List<PointF>();
      for (int i = 0; i < count; i++)
      {
        float x = paddingLeft + (float)(plotWidth * i / (count - 1 == 0 ? 1 : count - 1));
        float y = paddingTop + (float)(plotHeight - (plotHeight * (float)((Data[i].Value - min) / (max - min))));
        points.Add(new PointF(x, y));
      }

      // Linie: draw segment-by-segment, da DrawLines nicht existiert
      canvas.StrokeColor = Colors.DodgerBlue;
      canvas.StrokeSize = 2;
      if (points.Count > 1)
      {
        for (int i = 1; i < points.Count; i++)
        {
          var p0 = points[i - 1];
          var p1 = points[i];
          canvas.DrawLine(p0.X, p0.Y, p1.X, p1.Y);
        }
      }

      // Punkte
      canvas.FillColor = Colors.White;
      foreach (var p in points)
      {
        canvas.FillCircle(p.X, p.Y, 4);
        canvas.StrokeColor = Colors.DodgerBlue;
        canvas.DrawCircle(p.X, p.Y, 4);
      }

      // Einheit rechts oben
      if (!string.IsNullOrEmpty(YUnit))
      {
        canvas.FontSize = 10;
        canvas.FontColor = Colors.Gray;
        canvas.DrawString(YUnit, paddingLeft + plotWidth + 2, paddingTop - 2, 60, 16, HorizontalAlignment.Left, VerticalAlignment.Top);
      }

      canvas.RestoreState();
    }
  }
}