namespace LabourForceConnector.Console.Logger;

public sealed class ConsoleLogger : ILogger
{
    public void LogInformation(string message)
    {
        System.Console.WriteLine($"ÏNFO: {message}");
    }

    public void LogError(string message)
    {
        System.Console.WriteLine($"ERROR: {message}");
    }
    
    public void LogError(Exception message)
    {
        System.Console.WriteLine($"EXCEPTION: {message}");
    }
}