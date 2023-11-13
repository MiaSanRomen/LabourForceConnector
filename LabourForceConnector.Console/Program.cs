using LabourForceConnector.Console.Facade;
using LabourForceConnector.Console.Logger;
using LabourForceConnector.Console.Settings.Application;
using LabourForceConnector.Console.Settings.Files;
using LabourForceConnector.Console.Settings.Tables;
using LabourForceConnector.Console.Settings.Urls;

//downloaded .xlsx files are saved to Downloads folder
//converted .csv files are saved to Conversions folder

var appFacade = new ApplicationFacade(new ApplicationSettings(),
    new FilesSettings(),
    new TablesSettings(),
    new UrlSettings(),
    new ConsoleLogger());

await appFacade.TryNavigateToLinkAndDownloadFileAsync();

appFacade.GetDataTableFromDownloadsAndFilterRaws();










