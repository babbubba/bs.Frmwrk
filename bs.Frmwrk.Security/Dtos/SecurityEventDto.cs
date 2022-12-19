using bs.Frmwrk.Core.Dtos.Auth;

namespace bs.Frmwrk.Security.Dtos
{
    public class SecurityEventDto : ISecurityEventDto
    {
        public SecurityEventDto(string message, SecurityEventSeverity severity, string? userName = null, string? clientIp = null)
        {
            Message = message;
            Severity = severity;
            UserName = userName;
            ClientIp = clientIp;
        }

        public string? ClientIp { get; }

        public string Message { get; }

        public SecurityEventSeverity Severity { get; }

        public string? UserName { get; }
    }
}