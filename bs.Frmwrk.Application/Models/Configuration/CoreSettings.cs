﻿using bs.Frmwrk.Core.Models.Configuration;

namespace bs.Frmwrk.Application.Models.Configuration
{
    public class CoreSettings : ICoreSettings
    {
        public string? AppCompany { get; set; }
        public IDictionary<string, string>? AppRoles { get; set; }
        public string? AppTitle { get; set; }
        public string? CompanyWebSite { get; set; }
        public string? ExternalDllFilesRootPath { get; set; }
        public string? ExternalDllFilesSearchPattern { get; set; }
        public string? FrontendConfirmEmailUrl { get; set; }
        public string[]? FrontendOrigins { get; set; }
        public string? FrontendRecoveryPasswordUrl { get; set; }
        public string? PublishUrl { get; set; }
        public IDictionary<string, string>? SignalRHubs { get; set; }
    }
}