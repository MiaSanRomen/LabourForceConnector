using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using ExcelDataReader;
using LabourForceConnector.Console.Helpers.Files;
using LabourForceConnector.Console.Logger;
using LabourForceConnector.Console.Settings.Tables;

namespace LabourForceConnector.Console.Helpers.Tables;

public sealed class TablesHelper : ITablesHelper
{
    private readonly ITablesSettings _tablesSettings;
    private readonly IFileHelper _fileHelper;
    private readonly ILogger? _logger;

    public TablesHelper(ITablesSettings tablesSettings,
        IFileHelper fileHelper,
        ILogger? logger = null)
    {
        _tablesSettings = tablesSettings;
        _fileHelper = fileHelper;
        _logger = logger;
        
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }
    
    public bool TryGetDataTableFromDownloads(string fileName, 
        string tableName, 
        [NotNullWhen(true)] out DataTable? dataTable)
    {
        if (!_fileHelper.TryGetDownloadFileStream(fileName, out var fileStream)
            || !TryGetDataSetFromStream(fileStream, out var dataSet))
        {
            dataTable = default;
            return false;
        }
    
        dataTable = dataSet.Tables[tableName];
        return dataTable is not null;
    }
    
    public bool TryRemoveAllColumnsExceptDatesAndSeries(ref DataTable table)
    {
        try
        {
            DataRow[] rowsNames = new DataRow[table.Rows.Count];
            table.Rows.CopyTo(rowsNames, 0);
            for (var i = rowsNames.Length - 1; i >= 0; i--)
            {
                if(rowsNames[i][0] is DateTime)
                    continue;

                var rawName = rowsNames[i][0].ToString();
        
                if(!string.IsNullOrWhiteSpace(rawName)
                   && rawName.Contains(_tablesSettings.SeriesColumn))
                    continue;

                table.Rows.RemoveAt(i);
            }

            return true;
        }
        catch (Exception e)
        {
            _logger?.LogError(e);
            return false;
        }
    }
    
    public bool TryFormatAndSaveTableToCsvFile(string fileName, DataTable table)
    {
        return TryFormatTable(table, out var csvData) && TrySaveTableToCsvFile(csvData, fileName);
    }

    private bool TryFormatTable(DataTable table, [NotNullWhen(true)] out string? csvData)
    {
        try
        {
            var resultCsv = string.Empty;
            for (var i = 0; i < table.Columns.Count; i++)
            {
                for (int j = 0; j < table.Rows.Count; j++)
                {
                    var value = FormatCell(table.Rows[j][i]);
                    resultCsv += value + ",";
                }
        
                resultCsv  += "\n";
            }

            csvData = resultCsv;
            return true;
        }
        catch (Exception e)
        {
            _logger?.LogError(e);
            csvData = default;
            return false;
        }
    }

    private bool TrySaveTableToCsvFile(string csvData, string fileName)
    {
        return _fileHelper.TrySaveStringAsCsvToFile(csvData, fileName);
    }

    private string? FormatCell(object dataCell)
    {
        return dataCell is DateTime dateTime
            ? dateTime.ToString(_tablesSettings.DateFormat)
            : dataCell.ToString();
    }

    private bool TryGetDataSetFromStream(FileStream fileStream, [NotNullWhen(true)] out DataSet? dataSet)
    {
        var excelReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
        
        try
        {
            dataSet = excelReader.AsDataSet();
            excelReader.Close();
            return dataSet is not null;
        }
        catch (Exception e)
        {
            _logger?.LogError(e);
            dataSet = default;
            return false;
        }
        finally
        {
            excelReader.Close();
        }
    }
}