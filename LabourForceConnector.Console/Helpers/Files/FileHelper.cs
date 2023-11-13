using System.Diagnostics.CodeAnalysis;
using LabourForceConnector.Console.Logger;
using LabourForceConnector.Console.Settings.Files;

namespace LabourForceConnector.Console.Helpers.Files;

public sealed class FileHelper : IFileHelper
{
    private readonly IFilesSettings _filesSettings;
    private readonly ILogger? _logger;

    public FileHelper(IFilesSettings filesSettings,
        ILogger? logger = null)
    {
        _filesSettings = filesSettings;
        _logger = logger;
    }
    
    public bool TryGetDownloadFileStream(string fileName, [NotNullWhen(true)] out FileStream? fileStream)
    {
        if(!TryCreateDirectoryPath(_filesSettings.DownloadFolderName, out var directoryPath))
        {
            fileStream = default;
            return false;
        }
        
        Directory.CreateDirectory(directoryPath);
        var path = CreateFilePath(directoryPath, fileName);

        var result = TryGetFileStreamFromPath(path, out var fileStreamFromPath);
        fileStream = fileStreamFromPath;
        return result;
    }

    public bool TrySaveStringAsCsvToFile(string csvData, string fileName)
    {
        if(!TryCreateDirectoryPath(_filesSettings.ConversionFolderName, out var directoryPath))
            return false;
        
        Directory.CreateDirectory(directoryPath);
        var path = CreateFilePath(directoryPath, fileName);

        return TrySaveStringAsCsv(path, csvData);
    }

    private bool TryGetFileStreamFromPath(string path, [NotNullWhen(true)] out FileStream? fileStream)
    {
        try
        {
            fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            return true;
        }
        catch(Exception e)
        {
            _logger?.LogError(e);
            fileStream = default;
            return false;
        }
    }

    private bool TrySaveStringAsCsv(string path, string csvData)
    {
        var streamWriter = new StreamWriter(path, false);
        try
        {
            streamWriter.Write(csvData);
            return true;
        }
        catch (Exception e)
        {
            _logger?.LogError(e);
            return false;
        }
        finally
        {
            streamWriter.Close();
        }
    }

    private bool TryCreateDirectoryPath(string folderName, [NotNullWhen(true)] out string? directoryName)
    {
        try
        {
            var folderPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                @"..\..\..",
                folderName));
            
            var directoryResult = Path.GetDirectoryName(folderPath);
            if (string.IsNullOrWhiteSpace(directoryResult))
            {
                directoryName = string.Empty;
                return false;
            }
            
            directoryName = directoryResult;
            return true;
        }
        catch (Exception e)
        {
            _logger?.LogError(e);
            directoryName = string.Empty;
            return false;
        }
    }

    private string CreateFilePath(string directoryName, string fileName)
    {
        return Path.Combine(directoryName, fileName);
    }
}