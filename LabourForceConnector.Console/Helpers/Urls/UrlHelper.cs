using LabourForceConnector.Console.Helpers.Files;
using LabourForceConnector.Console.Logger;
using LabourForceConnector.Console.Settings;
using LabourForceConnector.Console.Settings.Urls;

namespace LabourForceConnector.Console.Helpers.Urls;

public sealed class UrlHelper : IUrlHelper, IDisposable
{
    private readonly IFileHelper _fileHelper;
    private readonly IUrlSettings _settings;
    private readonly ILogger? _logger;
    private readonly HttpClient _httpClient;
    
    public UrlHelper(IFileHelper fileHelper,
        IUrlSettings settings,
        ILogger? logger = null)
    {
        _fileHelper = fileHelper;
        _settings = settings;
        _logger = logger;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", _settings.UserAgent);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
    
    public async Task<string> GetHtmlFromUrlAsync(string url)
    {
        if (!IsFullUrl(url)) url = string.Concat(_settings.BaseUrl, url);
        return await GetUrlContentAsString(url);
    }
    
    public async Task<bool> TryDownloadFileFromUrlAsync(string url, string fileName)
    {
        if (!IsFullUrl(url)) url = string.Concat(_settings.BaseUrl, url);
        return await TryDownloadFileAsync(url, fileName);
    }

    private static bool IsFullUrl(string url)
    {
        return url.Contains("https://") || url.Contains("http://") || url.Contains("www.");
    }

    private async Task<string> GetUrlContentAsString(string url)
    {
        try
        {
            using var response = await _httpClient.GetAsync(url);
            using var content = response.Content;
            return await content.ReadAsStringAsync();
        }
        catch (Exception e)
        {
            _logger?.LogError(e);
            return string.Empty;
        }
    }

    private async Task<bool> TryDownloadFileAsync(string url, string fileName)
    {
        if (!_fileHelper.TryGetDownloadFileStream(fileName, out var fileStream)) return false;
        
        try
        {
            await using var httpStream = await _httpClient.GetStreamAsync(url);
            await httpStream.CopyToAsync(fileStream);
            return true;
        }
        catch (Exception e)
        {
            _logger?.LogError(e);
            return false;
        }
        finally
        {
            await fileStream.DisposeAsync();
        }
    }
}