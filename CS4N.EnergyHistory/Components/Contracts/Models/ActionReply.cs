namespace CS4N.EnergyHistory.Contracts.Models
{
  public class ActionReply
  {
    /// <summary>
    /// Erfolg
    /// </summary>
    public ActionReply()
    {
      Successful = true;
    }
    /// <summary>
    /// Fehlschlag
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <param name="errorTitle"></param>
    public ActionReply(string errorMessage, string errorTitle = "text_Error")
    {
      ErrorMessage = errorMessage;
      ErrorTitle = errorTitle;
    }

    public bool Successful { get; init; }
    public string? ErrorMessage { get; init; }
    public string? ErrorTitle { get; init; }
  }
}