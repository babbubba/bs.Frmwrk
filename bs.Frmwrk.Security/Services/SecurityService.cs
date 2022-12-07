using bs.Frmwrk.Core.Services.Security;

namespace bs.Frmwrk.Security.Services
{
    public class SecurityService : ISecurityService
    {
        public Task<bool> CheckPasswordValidity(string password, out string errorMessage)
        {
            //TODO: Implementa il check della validità della password
            throw new NotImplementedException();
        }

        public Task TrackLoginFail(string username, string clientIp)
        {
            //TODO: Implementa il traking degli accessi falliti
            throw new NotImplementedException();
        }
    }
}