using bs.Frmwrk.Core.Services.Locale;
using Microsoft.Extensions.Logging;
using bs.Data.Interfaces;
using AutoMapper;
using bs.Frmwrk.Core.ViewModels.Api;
using bs.Data;

namespace bs.Frmwrk.Base
{
    public class BaseService
    {
        protected readonly ILogger logger;
        private readonly ITranslateService translateService;
        protected readonly IUnitOfWork unitOfWork;

        public BaseService(ILogger logger, ITranslateService translateService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.logger = logger;
            this.translateService = translateService;
            this.unitOfWork = unitOfWork;
        }


        public async Task<IApiResponseViewModel> ExecuteTransactionAsync(Func<IApiResponseViewModel, Task<IApiResponseViewModel>> function, string genericErrorMessage)
        {
            unitOfWork.BeginTransaction();

            IApiResponseViewModel? response = default;

            if (function == null)
            {
                logger.LogError("Invalid function to execute. The function is null");
                response.Success = false;
                response.ErrorMessage = genericErrorMessage + System.Environment.NewLine + translateService.Translate("Errore eseguendo l'azione. L'azione è null.");
                return response;
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
                var error = genericErrorMessage + System.Environment.NewLine + ex.GetBaseException().Message;
                response.ErrorMessage = genericErrorMessage + System.Environment.NewLine + ex.GetBaseException().Message;
                response.Success = false;
                logger.LogError(ex, error);
            }

            return response;
        }

        public async Task<IApiResponseViewModel<TResponse>> ExecuteTransactionAsync<TResponse>(Func<IApiResponseViewModel<TResponse>, Task<IApiResponseViewModel<TResponse>>> function, string genericErrorMessage)
        {
            unitOfWork.BeginTransaction();

            IApiResponseViewModel<TResponse>? response = default;

            if (function == null)
            {
                logger.LogError("Invalid function to execute. The function is null");
                response.Success = false;
                response.ErrorMessage = genericErrorMessage + System.Environment.NewLine + translateService.Translate("Errore eseguendo l'azione. L'azione è null.");
                return response;
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
                var error = genericErrorMessage + System.Environment.NewLine + ex.GetBaseException().Message;
                response.ErrorMessage = genericErrorMessage + System.Environment.NewLine + ex.GetBaseException().Message;
                response.Success = false;
                logger.LogError(ex, error);
            }

            return response;
        }

    }
}