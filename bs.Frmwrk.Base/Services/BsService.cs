using bs.Data.Interfaces;
using bs.Frmwrk.Base.Dtos;
using bs.Frmwrk.Core.Dtos.Datatables;
using bs.Frmwrk.Core.Exceptions;
using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Services.Base;
using bs.Frmwrk.Core.Services.Locale;
using bs.Frmwrk.Core.Services.Mapping;
using bs.Frmwrk.Core.Services.Security;
using bs.Frmwrk.Core.ViewModels.Api;
using bs.Frmwrk.Shared;
using Microsoft.Extensions.Logging;
using NHibernate.Linq;

namespace bs.Frmwrk.Base.Services
{
    public abstract class BsService : IBsService
    {
        protected readonly ILogger logger;
        protected readonly IMapperService mapper;
        protected readonly ISecurityService securityService;
        protected readonly IUnitOfWork unitOfWork;
        private readonly ITranslateService translateService;

        public BsService(ILogger logger, ITranslateService translateService, IMapperService mapper, IUnitOfWork unitOfWork, ISecurityService securityService)
        {
            this.logger = logger;
            this.translateService = translateService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.securityService = securityService;
        }

        public async Task<IApiResponse<TResponse>> _ExecuteAsync<TResponse>(Func<IApiResponse<TResponse>, Task<IApiResponse<TResponse>>> function, string? genericErrorMessage = null)
        {
            IApiResponse<TResponse> response = (IApiResponse<TResponse>?)Activator.CreateInstance(typeof(ApiResponse<TResponse>)) ?? throw new BsException(2305180955, T("Impossibile costruire l'oggetto ApiResponse"));

            if (function == null)
            {
                return response.SetError(((genericErrorMessage != null) ? T(genericErrorMessage) + ": " : "") + T("Impossibile eseguire l'azione... la funzione non è valida."), 2305180956, logger);
            }

            try
            {
                response = await function.Invoke(response);
            }
            catch (BsException bex)
            {
                response = response.SetError(((genericErrorMessage != null) ? T(genericErrorMessage) + ": " : "") + bex.GetBaseException().Message, bex.ErrorCode, logger, bex);
            }
            catch (Exception ex)
            {
                response = response.SetError(((genericErrorMessage != null) ? T(genericErrorMessage) + ": " : "") + ex.GetBaseException().Message, null, logger, ex);
            }

            return response;
        }

        public async Task<IApiResponse> _ExecuteAsync(Func<IApiResponse, Task<IApiResponse>> function, string? genericErrorMessage = null)
        {
            IApiResponse response = (IApiResponse?)Activator.CreateInstance(typeof(ApiResponse)) ?? throw new BsException(2305180959, T("Impossibile costruire l'oggetto ApiResponse"));
            if (function == null)
            {
                return response.SetError(((genericErrorMessage != null) ? T(genericErrorMessage) + ": " : "") + T("Impossibile eseguire l'azione... la funzione non è valida."), 2305180957, logger);
            }

            try
            {
                response = await function.Invoke(response);
            }
            catch (BsException bex)
            {
                response = response.SetError(((genericErrorMessage != null) ? T(genericErrorMessage) + ": " : "") + bex.GetBaseException().Message, bex.ErrorCode, logger, bex);
            }
            catch (Exception ex)
            {
                response = response.SetError(((genericErrorMessage != null) ? T(genericErrorMessage) + ": " : "") + ex.GetBaseException().Message, null, logger, ex);
            }

            return response;
        }

        public async Task<IApiPagedResponse<TViewModel>> _ExecutePaginatedAsync<TSource, TViewModel>(IPageRequestDto pageRequest, IQueryable<TSource> source, Func<IQueryable<TSource>, IApiPagedResponse<TViewModel>, IQueryable<TSource>>? filterFuncion, IApiPagedResponse<TViewModel>? response)
        {
            response ??= (IApiPagedResponse<TViewModel>?)Activator.CreateInstance(typeof(ApiPagedResponse<TViewModel>)) ?? throw new BsException(2302061027, translateService.Translate("Impossibile costruire l'oggetto ApiPagedResponse"));

            var dto = pageRequest as PageRequestDto;

            var totalRecords = source.Count();

            // Set filter
            var filteredQuery = filterFuncion?.Invoke(source, response) ?? source;

            // Set order
            if (dto?.Order != null && dto.Order.Length > 0 && dto.Columns != null)
            {
                try
                {
                    var columnPropertyName = dto.Columns[dto.Order[0].Column].Name;
                    var orderDescending = dto.Order[0].Dir.ToLower() == "asc" ? false : true;
                    filteredQuery = filteredQuery.DynamicOrderNestedBy(columnPropertyName, orderDescending);
                }
                catch (Exception ex)
                {
                    return response.SetError(T("Errore eseguendo l'ordinamento dinamico della tabella: '{0}'", ex.GetBaseException().Message), 2305180940, logger, ex);
                }
            }

            //Retrieving data
            try
            {
                filteredQuery = filteredQuery.Distinct();
                response.Data = mapper.Map<IEnumerable<TViewModel>>(await filteredQuery.Skip(pageRequest.Start).Take(pageRequest.Length).ToListAsync());
                response.Draw = pageRequest.Draw;
                response.RecordsFiltered = filteredQuery.Count();
                response.RecordsTotal = totalRecords;
            }
            catch (Exception ex)
            {
                response = response.SetError(T("Errore ottenendo i dati della tabella: '{0}'", ex.GetBaseException().Message), 2305180941, logger, ex);
            }

            return response;
        }

        public async Task<IUserModel?> _GetSystemUser()
        {
            return await unitOfWork.Session.Query<IUserModel>().FirstOrDefaultAsync(u => u.IsSystemUser != null && u.IsSystemUser == true);
        }

        public async Task<IApiPagedResponse<TResponse>> ExecutePaginatedTransactionAsync<TSource, TResponse>(IPageRequestDto pageRequest, Func<IApiPagedResponse<TResponse>, IQueryable<TSource>> function, Func<IQueryable<TSource>, IApiPagedResponse<TResponse>, IQueryable<TSource>>? filterFuncion, string genericErrorMessage)
        {
            IApiPagedResponse<TResponse> response = (IApiPagedResponse<TResponse>?)Activator.CreateInstance(typeof(ApiPagedResponse<TResponse>)) ?? throw new BsException(2305180942, T("Impossibile costruire l'oggetto ApiPagedResponse"));

            if (function == null)
            {
                return response.SetError(T(genericErrorMessage) + ": " + T("Impossibile eseguire l'azione... la funzione non è valida."), 2305180943, logger);
            }

            try
            {
                unitOfWork.BeginTransaction();
                response = await _ExecutePaginatedAsync(pageRequest, function.Invoke(response), filterFuncion, response);
                if (response.Success)
                {
                    await unitOfWork.CommitAsync();
                }
                else
                {
                    await unitOfWork.RollbackAsync();
                }
            }
            catch (BsException bex)
            {
                await unitOfWork.RollbackAsync();
                response = response.SetError(T(genericErrorMessage) + ": " + bex.GetBaseException().Message, bex.ErrorCode, logger, bex);
            }
            catch (Exception ex)
            {
                response = response.SetError(T(genericErrorMessage) + ": " + ex.GetBaseException().Message, null, logger, ex);
            }

            return response;
        }

        /// <summary>
        /// Executes the transaction asynchronous and autorollback in case of error.
        /// </summary>
        /// <param name="function">The function.</param>
        /// <param name="genericErrorMessage">The generic error message.</param>
        /// <returns></returns>
        /// <exception cref="BsException">
        /// 2212071645
        /// or
        /// 2212071646
        /// </exception>
        public async Task<IApiResponse> ExecuteTransactionAsync(Func<IApiResponse, Task<IApiResponse>> function, string genericErrorMessage)
        {
            IApiResponse response = (IApiResponse?)Activator.CreateInstance(typeof(ApiResponse)) ?? throw new BsException(2305180951, T("Impossibile costruire l'oggetto ApiResponse"));

            try
            {
                unitOfWork.BeginTransaction();

                // try executing the operation method
                response = await _ExecuteAsync(function, genericErrorMessage);

                if (response.Success)
                {
                    await unitOfWork.CommitAsync();
                }
                else
                {
                    await unitOfWork.RollbackAsync();
                }
            }
            catch (BsException bex)
            {
                response = response.SetError(T(genericErrorMessage) + ": " + bex.GetBaseException().Message, bex.ErrorCode, logger, bex);
                await unitOfWork.RollbackAsync();
            }
            catch (Exception ex)
            {
                response = response.SetError(T(genericErrorMessage) + ": " + ex.GetBaseException().Message, null, logger, ex);
                await unitOfWork.RollbackAsync();
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
        /// <exception cref="BsException">
        /// 2212071647
        /// or
        /// 2212071646
        /// </exception>
        public async Task<IApiResponse<TResponse>> ExecuteTransactionAsync<TResponse>(Func<IApiResponse<TResponse>, Task<IApiResponse<TResponse>>> function, string genericErrorMessage)
        {
            IApiResponse<TResponse> response = (IApiResponse<TResponse>?)Activator.CreateInstance(typeof(ApiResponse<TResponse>)) ?? throw new BsException(2305180953, T("Impossibile costruire l'oggetto ApiResponse"));

            try
            {
                unitOfWork.BeginTransaction();
                response = await _ExecuteAsync(function, genericErrorMessage);
                if (response.Success)
                {
                    await unitOfWork.CommitAsync();
                }
                else
                {
                    await unitOfWork.RollbackAsync();
                }
            }
            catch (BsException bex)
            {
                response = response.SetError(T(genericErrorMessage) + ": " + bex.GetBaseException().Message, bex.ErrorCode, logger, bex);
                await unitOfWork.RollbackAsync();
            }
            catch (Exception ex)
            {
                response = response.SetError(T(genericErrorMessage) + ": " + ex.GetBaseException().Message, null, logger, ex);
                await unitOfWork.RollbackAsync();
            }

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