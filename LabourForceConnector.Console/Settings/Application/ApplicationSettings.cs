namespace LabourForceConnector.Console.Settings.Application;

public readonly record struct ApplicationSettings : IApplicationSettings
{
    public string MainPageUrl =>
        "https://www.abs.gov.au/statistics/labour/employment-and-unemployment/labour-force-australia";
    public string XPathTablesLink => "//div[contains(@class, 'view-display-id-topic_latest_release_block')]//a";
    public string XPathDownloadLink => "//a[starts-with(@aria-label, 'Table 1.')]";
    public string XlsxFileName => "data.xlsx";
    public string TableName => "Data1";
    public string CsvFileName => "data.csv";
}