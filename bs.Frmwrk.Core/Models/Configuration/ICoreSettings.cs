namespace bs.Frmwrk.Core.Models.Configuration
{
    public interface ICoreSettings
    {
        string ExternalDllFilesSearchPattern { get; set; }
        string ExternalDllFilesRootPath { get; set; }
    }
}
