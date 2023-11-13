using System.Diagnostics.CodeAnalysis;

namespace LabourForceConnector.Console.Helpers.Html;

public interface IHtmlElementHelper
{
    public bool TryGetUrlByElementXPath(string pageContent, string xPath, [NotNullWhen(true)] out string? url);
}