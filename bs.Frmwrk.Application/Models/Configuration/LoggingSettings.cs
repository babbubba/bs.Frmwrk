using bs.Frmwrk.Core.Models.Configuration;

namespace bs.Frmwrk.Application.Models.Configuration
{
    public class LoggingSettings : ILoggingSettings
    {
        public string? Path { get; set; }
        public string? LogFileName { get; set; }
        public bool? Debug { get; set; }
        public int? RetentionDays { get; set; }
        public int? LogFileLimitInBytes { get; set; }
        public string? Template { get; set; }
        public string? SeqEndpoint { get; set; }
        public string? ApplicationName { get; set; }
    }
}