using bs.Frmwrk.Core.ViewModels.Api;

namespace bs.Frmwrk.Core.Services.Base
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBsService
    {
        /// <summary>
        /// Executes the datatable asynchronous.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="genericErrorMessage">The generic error message.</param>
        /// <returns></returns>
        Task<IApiPagedResponse<TResponse>> ExecuteDatatableAsync<TResponse>(Func<IApiPagedResponse<TResponse>, Task<IApiPagedResponse<TResponse>>> function, string genericErrorMessage);

        /// <summary>
        /// Executes the transaction asynchronous.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <param name="genericErrorMessage">The generic error message.</param>
        /// <returns></returns>
        Task<IApiResponse> ExecuteTransactionAsync(Func<IApiResponse, Task<IApiResponse>> function, string genericErrorMessage);

        /// <summary>
        /// Executes the transaction asynchronous.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="genericErrorMessage">The generic error message.</param>
        /// <returns></returns>
        Task<IApiResponse<TResponse>> ExecuteTransactionAsync<TResponse>(Func<IApiResponse<TResponse>, Task<IApiResponse<TResponse>>> function, string genericErrorMessage);

        /// <summary>
        /// ts the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        string T(string text);

        /// <summary>
        /// ts the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="objs">The objs.</param>
        /// <returns></returns>
        string T(string text, params object[] objs);
    }
}