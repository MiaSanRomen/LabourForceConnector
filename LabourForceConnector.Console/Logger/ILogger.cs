namespace LabourForceConnector.Console.Logger;

public interface ILogger
{
    public void LogInformation(string message);
    public void LogError(string message);
    public void LogError(Exception message);
}