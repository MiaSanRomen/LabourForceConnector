using System.Diagnostics.CodeAnalysis;

namespace LabourForceConnector.Console.Helpers.Files;

public interface IFileHelper
{
    bool TryGetDownloadFileStream(string fileName, [NotNullWhen(true)] out FileStream? fileStream);
    bool TrySaveStringAsCsvToFile(string csvData, string fileName);
}