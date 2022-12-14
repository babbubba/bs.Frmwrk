namespace bs.Frmwrk.Core.Models.Configuration
{
    public interface ICoreSettings
    {
        string? ExternalDllFilesSearchPattern { get; set; }
        string? ExternalDllFilesRootPath { get; set; }
        string[]? FrontendOrigins { get; set; }
        string? AppTitle { get; set; }
        string? AppCompany { get; set; }
        string? CompanyWebSite { get; set; }
        IDictionary<string, string>? AppRoles { get; set; }
    }
}
