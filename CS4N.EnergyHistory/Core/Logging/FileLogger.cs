using Microsoft.Extensions.Logging;

namespace CS4N.EnergyHistory.Core.Logging
{

  public sealed class FileLogger : ILogger
  {
    public LogLevel LogLevel { get; init; } = LogLevel.Debug;

    public string LogFolderPath => Path.Combine(PathHelper.GetWorkPath(), "Log");
    public string LogFilePath => Path.Combine(LogFolderPath, $"{DateTime.Today:yyyy-MM-dd}.log");

    private object _locker = new object();

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

    public bool IsEnabled(LogLevel logLevel)
    {
      return LogLevel <= logLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
      if (LogLevel > logLevel)
        return;

      if (!Directory.Exists(LogFolderPath))
        Directory.CreateDirectory(LogFolderPath);

      lock (_locker)
      {
        //File.AppendAllText(
        File.AppendAllLines(LogFilePath, [$"{DateTime.Now:HH:mm:ss.fff}|{logLevel}|{formatter(state, exception)}"]);
      }
    }
  }
}