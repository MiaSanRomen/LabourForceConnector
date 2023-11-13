using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace LabourForceConnector.Console.Helpers.Tables;

public interface ITablesHelper
{
    bool TryGetDataTableFromDownloads(string fileName,
        string tableName,
        [NotNullWhen(true)] out DataTable? dataTable);

    bool TryRemoveAllColumnsExceptDatesAndSeries(ref DataTable table);

    bool TryFormatAndSaveTableToCsvFile(string fileName, DataTable table);
}