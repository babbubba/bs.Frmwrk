using bs.Data.Interfaces;
using bs.Frmwrk.Base.Services;
using bs.Frmwrk.Core.Dtos.Navigation;
using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Models.Navigation;
using bs.Frmwrk.Core.Services.Base;
using bs.Frmwrk.Core.Services.Locale;
using bs.Frmwrk.Core.Services.Mapping;
using bs.Frmwrk.Core.Services.Navigation;
using bs.Frmwrk.Core.Services.Security;
using bs.Frmwrk.Core.ViewModels.Api;
using bs.Frmwrk.Core.ViewModels.Navigation;
using bs.Frmwrk.Shared;
using Microsoft.Extensions.Logging;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace bs.Frmwrk.Navigation.Services
{
    public class NavigationService : BsService, INavigationService, IInitializableService
    {
        public NavigationService(ILogger<NavigationService> logger, ITranslateService translateService, IMapperService mapper, IUnitOfWork unitOfWork, ISecurityService securityService) : base(logger, translateService, mapper, unitOfWork, securityService)
        {
        }

        public async Task<IApiResponse<IMenuViewModel>> _CreateUpdateMenuAsync(ICreateMenuDto dto, IUserModel currentUser)
        {
            return await _ExecuteAsync<IMenuViewModel>(async (response) =>
            {
                IMenuModel? model = (dto.Id != null) 
                ? await unitOfWork.Session.Query<IMenuModel>().SingleOrDefaultAsync(x => x.Id == dto.Id.ToGuid()) 
                : await unitOfWork.Session.Query<IMenuModel>().FirstOrDefaultAsync(x => x.Code == dto.Code);

                if (model == null)
                {
                    //create
                    model = mapper.Map<IMenuModel>(dto);
                }
                else
                {
                    //update
                    dto.Id ??= model.Id.ToString();
                    model = mapper.Map(dto, model);
                }

                await unitOfWork.Session.SaveOrUpdateAsync(model);

                response.Value = mapper.Map<IMenuViewModel>(model);

                return response;
            });
        }

        public async Task<IApiResponse<IMenuViewModel>> CreateUpdateMenuAsync(ICreateMenuDto menu, IUserModel currentUser)
        {
            return await ExecuteTransactionAsync<IMenuViewModel>(async (response) =>
            {
                return await _CreateUpdateMenuAsync(menu, currentUser);
            }, "Errore creando o aggiornando il menu");
        }

        public async Task<IApiResponse<IMenuItemViewModel>> CreateUpdateMenuItemAsync(ICreateMenuItemDto menuItem, IUserModel currentUser)
        {
            return await ExecuteTransactionAsync<IMenuItemViewModel>(async (response) =>
            {
                return await _CreateUpdateMenuItemAsync(menuItem, currentUser);
            }, "Errore creando o aggiornando la voce di menu");
        }

        public async Task<IApiResponse<IMenuItemViewModel>> _CreateUpdateMenuItemAsync(ICreateMenuItemDto dto, IUserModel currentUser)
        {
            return await _ExecuteAsync<IMenuItemViewModel>(async (response) =>
            {
                IMenuItemModel? model = (dto.Id == null)
                ? await unitOfWork.Session.Query<IMenuItemModel>().SingleOrDefaultAsync(x => x.Code == dto.Code && x.ParentMenu.Code == dto.ParentMenuCode)
                : await unitOfWork.Session.Query<IMenuItemModel>().SingleOrDefaultAsync(x => x.Id == dto.Id.ToGuid());


                if (model == null)
                {
                    //create
                    model = mapper.Map<IMenuItemModel>(dto);
                }
                else
                {
                    //update
                    dto.Id ??= model.Id.ToString();
                    model = mapper.Map(dto, model);
                }

                // ParentMenu
                var parentMenu = await unitOfWork.Session.Query<IMenuModel>().SingleOrDefaultAsync(x => x.Code == dto.ParentMenuCode);
                if (parentMenu == null)
                {
                    return response.SetError(T("Il menu con codice '{0}' non esiste", dto.ParentMenuCode), 2305181423, logger);
                }
                model.ParentMenu = parentMenu;

                // *ParentItem
                if (dto.ParentItemCode != null)
                {
                    var parentMenuItem = await unitOfWork.Session.Query<IMenuItemModel>().SingleOrDefaultAsync(x => x.Code == dto.ParentItemCode);
                    if (parentMenuItem == null)
                    {
                        return response.SetError(T("La voce di menu con codice '{0}' non esiste", dto.ParentItemCode), 2305181424, logger);
                    }
                    model.ParentItem = parentMenuItem;
                }

                // *AuthorizedRoles
                if (dto.AuthorizedRolesCode != null && dto.AuthorizedRolesCode.Any())
                {
                    var roles = await unitOfWork.Session.QueryOver<IRoleModel>().Where(x => x.Code.IsIn(dto.AuthorizedRolesCode)).ListAsync();
                    model.AuthorizedRoles ??= new List<IRoleModel>();
                    model.AuthorizedRoles.UpdateLists(roles, r=>r.Code);
                }

                // *RequiredPermissions
                if (dto.RequiredPermissionsCode != null && dto.RequiredPermissionsCode.Any())
                {
                    var permissions = await unitOfWork.Session.QueryOver<IPermissionModel>().Where(x => x.Code.IsIn(dto.RequiredPermissionsCode)).ListAsync();
                    model.RequiredPermissions ??= new List<IPermissionModel>();
                    model.RequiredPermissions.UpdateLists(permissions, p=>p.Code);
                }

                await unitOfWork.Session.SaveOrUpdateAsync(model);

                response.Value = mapper.Map<IMenuItemViewModel>(model);

                return response;
            });
        }

        public Task<IApiResponse> DeleteMenuAsync(string menuItemId, IUserModel currentUser)
        {
            throw new NotImplementedException();
        }

        public Task<IApiResponse> DeleteMenuItemAsync(string menuItemId, IUserModel currentUser)
        {
            throw new NotImplementedException();
        }

        public Task<IApiResponse<IMenuViewModel?>> GetMenuByCodeAsync(string menuCode, IUserModel currentUser)
        {
            throw new NotImplementedException();
        }

        public Task<IApiResponse<IMenuItemViewModel?>> GetMenuItemByCodeAsync(string menuItemCode, IUserModel currentUser)
        {
            throw new NotImplementedException();
        }

        public Task<IApiResponse<IMenuItemViewModel[]?>> GetMenuItemsByMenuCodeAsync(string menuCode, IUserModel currentUser)
        {
            throw new NotImplementedException();
        }

        public async Task<IApiResponse> InitServiceAsync()
        {
            return new ApiResponse();
            //throw new NotImplementedException();
            //TODO: Implementa
        }
    }
}