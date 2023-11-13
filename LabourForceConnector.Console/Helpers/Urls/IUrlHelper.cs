namespace LabourForceConnector.Console.Helpers.Urls;

public interface IUrlHelper
{
    Task<string> GetHtmlFromUrlAsync(string url);
    Task<bool> TryDownloadFileFromUrlAsync(string url, string fileName);
}