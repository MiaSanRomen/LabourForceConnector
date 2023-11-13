namespace LabourForceConnector.Console.Settings.Files;

public readonly record struct FilesSettings : IFilesSettings
{
    public string DownloadFolderName => "Downloads/";
    public string ConversionFolderName => "Conversions/";
}