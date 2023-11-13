using System.Diagnostics.CodeAnalysis;
using HtmlAgilityPack;
using LabourForceConnector.Console.Logger;

namespace LabourForceConnector.Console.Helpers.Html;

public sealed class HtmlElementHelper : IHtmlElementHelper
{
    private readonly ILogger? _logger;

    public HtmlElementHelper(ILogger? logger = null)
    {
        _logger = logger;
    }
    
    public bool TryGetUrlByElementXPath(string pageContent, string xPath, [NotNullWhen(true)] out string? url)
    {
        try
        {
            var htmlSnippet = new HtmlDocument();
            htmlSnippet.LoadHtml(pageContent);
            var linkElement = htmlSnippet.DocumentNode.SelectSingleNode(xPath);
            url = linkElement.Attributes["href"].Value;
            return true;
        }
        catch (Exception e)
        {
            _logger?.LogError(e);
            url = default;
            return false;
        }
    }
}