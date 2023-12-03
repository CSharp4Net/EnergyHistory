namespace CS4N.EnergyHistory.Core
{
  public static class PathHelper
  {
    public static string GetRootPath()
    {
#if !RELEASE
      return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "wwwroot"));
#else
      return Path.Combine(AppContext.BaseDirectory, "wwwroot");
#endif
    }
  }
}