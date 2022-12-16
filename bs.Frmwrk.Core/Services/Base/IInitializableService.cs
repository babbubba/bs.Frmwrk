using bs.Frmwrk.Core.ViewModels.Api;

namespace bs.Frmwrk.Core.Services.Base
{
    /// <summary>
    /// 
    /// </summary>
    public interface IInitializableService
    {
        /// <summary>
        /// Initializes the service asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<IApiResponse> InitServiceAsync();
    }
}