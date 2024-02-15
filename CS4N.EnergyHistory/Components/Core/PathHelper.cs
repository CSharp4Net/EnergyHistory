using CS4N.EnergyHistory.Contracts;

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

    public static string GetWorkPath()
    {
      string workFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Constants.ApplicationName);

      if (!Directory.Exists(workFolderPath))
        Directory.CreateDirectory(workFolderPath);

      return workFolderPath;
    }
  }
}