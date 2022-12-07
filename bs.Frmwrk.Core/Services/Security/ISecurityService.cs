namespace bs.Frmwrk.Core.Services.Security
{
    public interface ISecurityService
    {
        Task TrackLoginFail(string username, string clientIp);

        Task<bool> CheckPasswordValidity(string password, out string errorMessage);
    }
}