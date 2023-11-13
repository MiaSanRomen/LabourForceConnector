namespace LabourForceConnector.Console.Settings.Urls;

public readonly record struct UrlSettings : IUrlSettings
{
    public string BaseUrl => "https://www.abs.gov.au";
    public string UserAgent => "LabourForceConnector";
}