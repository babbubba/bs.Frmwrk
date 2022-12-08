namespace bs.Frmwrk.Core.Services.Security
{
    public interface ISecurityService
    {
        Task TrackLoginFailAsync(string username, string clientIp);

        bool CheckPasswordValidity(string password, out string? errorMessage);
    }
}