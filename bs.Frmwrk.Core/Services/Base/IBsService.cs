using bs.Frmwrk.Core.ViewModels.Api;

namespace bs.Frmwrk.Core.Services.Base
{
    public interface IBsService
    {
        Task<IApiPagedResponse<TResponse>> ExecuteDatatableAsync<TResponse>(Func<IApiPagedResponse<TResponse>, Task<IApiPagedResponse<TResponse>>> function, string genericErrorMessage);

        Task<IApiResponse> ExecuteTransactionAsync(Func<IApiResponse, Task<IApiResponse>> function, string genericErrorMessage);

        Task<IApiResponse<TResponse>> ExecuteTransactionAsync<TResponse>(Func<IApiResponse<TResponse>, Task<IApiResponse<TResponse>>> function, string genericErrorMessage);

        string T(string text);

        string T(string text, params object[] objs);
    }
}