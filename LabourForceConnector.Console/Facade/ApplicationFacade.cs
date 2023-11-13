using LabourForceConnector.Console.Helpers.Files;
using LabourForceConnector.Console.Helpers.Html;
using LabourForceConnector.Console.Helpers.Tables;
using LabourForceConnector.Console.Helpers.Urls;
using LabourForceConnector.Console.Logger;
using LabourForceConnector.Console.Settings.Application;
using LabourForceConnector.Console.Settings.Files;
using LabourForceConnector.Console.Settings.Tables;
using LabourForceConnector.Console.Settings.Urls;

namespace LabourForceConnector.Console.Facade;

public class ApplicationFacade
{
    private readonly IApplicationSettings _applicationSettings;
    private readonly ITablesHelper _tablesHelper;
    private readonly IUrlHelper _urlHelper;
    private readonly IHtmlElementHelper _htmlElementHelper;
    private readonly ILogger? _logger;

    public ApplicationFacade(IApplicationSettings applicationSettings,
        IFilesSettings filesSettings,
        ITablesSettings tablesSettings,
        IUrlSettings urlSettings,
        ILogger? logger = null)
    {
        _applicationSettings = applicationSettings;
        _logger = logger;

        var fileHelper = new FileHelper(filesSettings, logger);
        _urlHelper = new UrlHelper(fileHelper, urlSettings, logger);
        _htmlElementHelper = new HtmlElementHelper(logger);
        _tablesHelper = new TablesHelper(tablesSettings, fileHelper, logger);
    }

    public async Task TryNavigateToLinkAndDownloadFileAsync()
    {
        var mainPageContent = await _urlHelper.GetHtmlFromUrlAsync(_applicationSettings.MainPageUrl);
        if (string.IsNullOrWhiteSpace(mainPageContent))
        {
            _logger?.LogError("First page is not parsed");
            return;
        }
        
        _logger?.LogInformation("First page parsed");
        
        if (!_htmlElementHelper.TryGetUrlByElementXPath(mainPageContent,
                _applicationSettings.XPathTablesLink, 
                out var tablesPageUrl))
        {
            _logger?.LogError("Second page url is not found");
            return;
        }

        _logger?.LogInformation("Second page url is found");

        var tablesPageContent = await _urlHelper.GetHtmlFromUrlAsync(tablesPageUrl);
        if (string.IsNullOrWhiteSpace(tablesPageContent))
        {
            _logger?.LogError("Second page is not parsed");
            return;
        }
        
        _logger?.LogInformation("Second page is parsed");
        
        if (!_htmlElementHelper.TryGetUrlByElementXPath(tablesPageContent, 
                _applicationSettings.XPathDownloadLink, 
                out var downloadUrl))
        {
            _logger?.LogError("Download url is found");
            return;
        }
        
        _logger?.LogInformation("Download url is found");
        
        if(!await _urlHelper.TryDownloadFileFromUrlAsync(downloadUrl!, _applicationSettings.XlsxFileName))
        {
            _logger?.LogError("Download file from url is failed");
            return;
        }
        
        _logger?.LogInformation("Excel file is downloaded");
    }

    public void GetDataTableFromDownloadsAndFilterRaws()
    {
        if (!_tablesHelper.TryGetDataTableFromDownloads(_applicationSettings.XlsxFileName,
                _applicationSettings.TableName,
                out var table))
        {
            _logger?.LogError("Get dataTable from downloads is failed");
            return;
        }
        
        _logger?.LogInformation("Table is parsed");

        if (!_tablesHelper.TryRemoveAllColumnsExceptDatesAndSeries(ref table))
        {
            _logger?.LogError("Get dataTable from downloads is failed");
            return;
        }
        
        _logger?.LogInformation("Table rows are filtered");
        
        if(!_tablesHelper.TryFormatAndSaveTableToCsvFile(_applicationSettings.CsvFileName, table))
        {
            _logger?.LogError("Format or save table to csv file is failed");
            return;
        }
        
        _logger?.LogInformation("CSV file is saved");
    }
}