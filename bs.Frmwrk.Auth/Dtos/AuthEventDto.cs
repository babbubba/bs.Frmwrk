using bs.Frmwrk.Core.Dtos.Auth;

namespace bs.Frmwrk.Auth.Dtos
{


    public class AuthEventDto : IAuthEventDto
    {
        public AuthEventDto(string userName, bool success, string message, string? clientIp)
        {
            UserName = userName;
            Message = message;
            Success = success;
            ClientIp = clientIp;
        }

        public string Message { get; }
        public bool Success { get; }
        public string UserName { get; }
        public string? ClientIp { get; }
    }
}