using bs.Frmwrk.Core.Dtos.Datatables;
using bs.Frmwrk.Core.ViewModels.Api;

namespace bs.Frmwrk.Core.Services.Base
{
    /// <summary>
    ///
    /// </summary>
    public interface IBsService
    {
        /// <summary>Executes the paginated transaction asynchronous.</summary>
        /// <typeparam name="TSource">The type of the entity Model to query.</typeparam>
        /// <typeparam name="TResponse">The type of the View Model corresponding to the entity model.</typeparam>
        /// <param name="pageRequest">The page request DTO (in the Datatables.Net format).</param>
        /// <param name="function">The function that returns the Queryable object of entities Models</param>
        /// <param name="filterFuncion">The optional filter funcion that return the Queryable object of entities Models filtered.</param>
        /// <param name="genericErrorMessage">The error message to return if exception occurrs.</param>
        /// <returns>The ApiPagedResponse View Model containing paged data</returns>
        Task<IApiPagedResponse<TResponse>> ExecutePaginatedTransactionAsync<TSource, TResponse>(IPageRequestDto pageRequest, Func<IQueryable<TSource>> function, Func<IQueryable<TSource>, IQueryable<TSource>>? filterFuncion, string genericErrorMessage);

        /// <summary>
        ///   <para>
        /// Executes the function in an asynchronous transaction and automatically commit at the end. </para>
        ///   <para>If exception occurrs it automatically rollback transaction and prepare the safe response to sen to frontend.
        /// </para>
        /// </summary>
        /// <param name="function">The function to execute.</param>
        /// <param name="genericErrorMessage">The error message used if an exception occurs processing the function</param>
        /// <returns>The ApiResponse view model containing success or error message</returns>
        Task<IApiResponse> ExecuteTransactionAsync(Func<IApiResponse, Task<IApiResponse>> function, string genericErrorMessage);

        /// <summary>
        ///   <para>
        /// Executes the function in an asynchronous transaction and automatically commit at the end. </para>
        ///   <para>If exception occurrs it automatically rollback transaction and prepare the safe response to sen to frontend.
        /// </para>
        /// </summary>
        /// <typeparam name="TResponse">The type of the response value.</typeparam>
        /// <param name="function">The function to execute.</param>
        /// <param name="genericErrorMessage">The error message used if an exception occurs processing the function.</param>
        /// <returns>The ApiResponse view model whit the response value or error message</returns>
        Task<IApiResponse<TResponse>> ExecuteTransactionAsync<TResponse>(Func<IApiResponse<TResponse>, Task<IApiResponse<TResponse>>> function, string genericErrorMessage);

        /// <summary>Paginates the asynchronous.</summary>
        /// <typeparam name="TSource">The type of the Queryable entity object used as source.</typeparam>
        /// <typeparam name="TViewModel">The type of the view model used to map the entity object in the response.</typeparam>
        /// <param name="pageRequest">The page request dto (it is used by datatables.net).</param>
        /// <param name="source">The Queryable entities used as source.</param>
        /// <param name="filterFuncion">Optional filter function. If null no filter will be applied.</param>
        /// <returns>It returns the paginated view model of the mapped entity in the format valid for datatables.net</returns>
        Task<IApiPagedResponse<TViewModel>> _ExecutePaginatedAsync<TSource, TViewModel>(IPageRequestDto pageRequest, IQueryable<TSource> source, Func<IQueryable<TSource>, IQueryable<TSource>>? filterFuncion);

        /// <summary>
        /// Translates the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        string T(string text);

        /// <summary>
        /// Translates the string using string format and parametric objects.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="objs">The objs.</param>
        /// <returns></returns>
        string T(string text, params object[] objs);

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="genericErrorMessage">The generic error message.</param>
        /// <returns></returns>
        Task<IApiResponse<TResponse>> _ExecuteAsync<TResponse>(Func<IApiResponse<TResponse>, Task<IApiResponse<TResponse>>> function, string? genericErrorMessage = null);
        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <param name="genericErrorMessage">The generic error message.</param>
        /// <returns></returns>
        Task<IApiResponse> _ExecuteAsync(Func<IApiResponse, Task<IApiResponse>> function, string? genericErrorMessage = null);
    }
}