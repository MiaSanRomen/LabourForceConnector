namespace LabourForceConnector.Console.Settings.Urls;

public interface IUrlSettings
{
    public string BaseUrl { get; }
    public string UserAgent { get; }
}