using bs.Frmwrk.Core.Services.Locale;
using Microsoft.Extensions.Logging;
using bs.Data.Interfaces;
using AutoMapper;
using bs.Frmwrk.Core.ViewModels.Api;
using bs.Data;
using bs.Frmwrk.Base.Exceptions;
using bs.Frmwrk.Core.Services.Base;

namespace bs.Frmwrk.Base
{


    public abstract class BaseService : IBaseService
    {
        protected readonly ILogger logger;
        private readonly ITranslateService translateService;
        protected readonly IMapper mapper;
        protected readonly IUnitOfWork unitOfWork;

        public BaseService(ILogger logger, ITranslateService translateService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.logger = logger;
            this.translateService = translateService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Executes the transaction asynchronous and autorollback in case of error.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <param name="genericErrorMessage">The generic error message.</param>
        /// <returns></returns>
        /// <exception cref="bs.Frmwrk.Base.Exceptions.BsException">
        /// 2212071645
        /// or
        /// 2212071646
        /// </exception>
        public async Task<IApiResponseViewModel> ExecuteTransactionAsync(Func<IApiResponseViewModel, Task<IApiResponseViewModel>> function, string genericErrorMessage)
        {
            unitOfWork.BeginTransaction();

            IApiResponseViewModel? response = default;

            if (response == null)
            {
                throw new BsException(2212071645, translateService.Translate("Impossibile costruire l'oggetto ApiResponse."));
            }

            if (function == null)
            {
                throw new BsException(2212071646, translateService.Translate("Impossibile eseguire l'azione. La funzione non è valida."));
            }

            try
            {
                // try executing the operation method
                response = await function.Invoke(response);//.ConfigureAwait(false);
                await unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                response.ErrorMessage = translateService.Translate(genericErrorMessage) + Environment.NewLine + ex.GetBaseException().Message;
                response.Success = false;
                logger.LogError(ex, response.ErrorMessage);
            }

            return response;
        }

        /// <summary>
        /// Executes the transaction asynchronous and autorollback in case of error.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="genericErrorMessage">The generic error message.</param>
        /// <returns></returns>
        /// <exception cref="bs.Frmwrk.Base.Exceptions.BsException">
        /// 2212071647
        /// or
        /// 2212071646
        /// </exception>
        public async Task<IApiResponseViewModel<TResponse>> ExecuteTransactionAsync<TResponse>(Func<IApiResponseViewModel<TResponse>, Task<IApiResponseViewModel<TResponse>>> function, string genericErrorMessage)
        {
            unitOfWork.BeginTransaction();

            IApiResponseViewModel<TResponse>? response = default;
            if (response == null)
            {
                throw new BsException(2212071647, translateService.Translate("Impossibile costruire l'oggetto ApiResponse"));
            }

            if (function == null)
            {
                throw new BsException(2212071646, translateService.Translate("Impossibile eseguire l'azione. La funzione non è valida."));
            }

            try
            {
                response = await function.Invoke(response);
                await unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                response.ErrorMessage = translateService.Translate(genericErrorMessage) + Environment.NewLine + ex.GetBaseException().Message;
                response.Success = false;
                logger.LogError(ex, response.ErrorMessage);
            }

            return response;
        }

        /// <summary>
        /// Executes the datatable asynchronous.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="genericErrorMessage">The generic error message.</param>
        /// <returns></returns>
        /// <exception cref="bs.Frmwrk.Base.Exceptions.BsException">
        /// 2212071652
        /// or
        /// 2212071653
        /// </exception>
        public async Task<IApiPagedResponseViewModel<TResponse>> ExecuteDatatableAsync<TResponse>(
         Func<IApiPagedResponseViewModel<TResponse>, Task<IApiPagedResponseViewModel<TResponse>>> function,
         string genericErrorMessage)
        {
            IApiPagedResponseViewModel<TResponse>? response = default;

            if (response == null)
            {
                throw new BsException(2212071652, translateService.Translate("Impossibile costruire l'oggetto ApiResponse"));
            }

            if (function == null)
            {
                throw new BsException(2212071653, translateService.Translate("Impossibile eseguire l'azione. La funzione non è valida."));
            }

            try
            {
                // try executing the operation method
                response = await function.Invoke(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                response.ErrorMessage = translateService.Translate(genericErrorMessage) + Environment.NewLine + ex.GetBaseException().Message;
                response.Success = false;
                logger.LogError(ex, response.ErrorMessage);
            }

            // return the response
            return response;
        }

        /// <summary>
        /// ts the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public string T(string text)
        {
            return translateService.Translate(text);
        }

        /// <summary>
        /// Translate the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="objs">The objs.</param>
        /// <returns></returns>
        public string T(string text, params object[] objs)
        {
            return translateService.Translate(text, objs);
        }
    }
}