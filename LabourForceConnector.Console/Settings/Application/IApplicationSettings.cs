namespace LabourForceConnector.Console.Settings.Application;

public interface IApplicationSettings
{
    public string MainPageUrl { get; }
    public string XPathTablesLink { get; }
    public string XPathDownloadLink { get; }
    public string XlsxFileName { get; }
    public string TableName { get; }
    public string CsvFileName { get; }
}