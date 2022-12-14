using bs.Frmwrk.Core.Models.Configuration;

namespace bs.Frmwrk.Application.Models.Configuration
{
    public class CoreSettingsModel : ICoreSettings
    {
        public string ExternalDllFilesSearchPattern { get; set; }
        public string ExternalDllFilesRootPath { get; set; }
    }
}