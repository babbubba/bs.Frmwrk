using bs.Frmwrk.Core.ViewModels.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Frmwrk.Core.Services.Base
{
    public interface IBsService
    {
        Task<IApiPagedResponseViewModel<TResponse>> ExecuteDatatableAsync<TResponse>(Func<IApiPagedResponseViewModel<TResponse>, Task<IApiPagedResponseViewModel<TResponse>>> function, string genericErrorMessage);
        Task<IApiResponseViewModel> ExecuteTransactionAsync(Func<IApiResponseViewModel, Task<IApiResponseViewModel>> function, string genericErrorMessage);
        Task<IApiResponseViewModel<TResponse>> ExecuteTransactionAsync<TResponse>(Func<IApiResponseViewModel<TResponse>, Task<IApiResponseViewModel<TResponse>>> function, string genericErrorMessage);
        string T(string text);
        string T(string text, params object[] objs);
    }
}
