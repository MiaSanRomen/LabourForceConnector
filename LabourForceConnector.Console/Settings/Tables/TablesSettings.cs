namespace LabourForceConnector.Console.Settings.Tables;

public readonly record struct TablesSettings : ITablesSettings
{
    public string SeriesColumn => "Series ID";
    public string DateFormat => "MMM-yy";
}