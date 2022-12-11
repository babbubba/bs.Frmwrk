namespace bs.Frmwrk.Core.Models.Configuration
{
    public interface ILoggingSettings
    {
        string? Path { get; set; }
        string? LogFileName { get; set; }
        bool? Debug { get; set; }
        int? RetentionDays { get; set; }
        int? LogFileLimitInBytes { get; set; }
        string? Template { get; set; }
        string? SeqEndpoint { get; set; }
        string? ApplicationName { get; set; }
    }

}
