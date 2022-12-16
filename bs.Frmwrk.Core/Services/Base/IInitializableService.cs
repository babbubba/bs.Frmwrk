using bs.Frmwrk.Core.ViewModels.Api;

namespace bs.Frmwrk.Core.Services.Base
{
    public interface IInitializableService
    {
        Task<IApiResponse> InitServiceAsync();
    }
}