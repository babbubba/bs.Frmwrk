using bs.Data.Interfaces;
using bs.Frmwrk.Base.Services;
using bs.Frmwrk.Core.Dtos.Navigation;
using bs.Frmwrk.Core.Globals.Security;
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
using Mysqlx.Session;
using MySqlX.XDevAPI.Common;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Util;
using static System.Net.Mime.MediaTypeNames;

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
                if (!await securityService.CheckUserPermissionAsync(currentUser as IPermissionedUser, PermissionsCodes.MENUS_REGISTRY))
                {
                    return response.SetError(T("Autorizzazioni non sufficenti per creare/modificare un  menu"), 2305191148, logger);
                }

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
                if(! await securityService.CheckUserPermissionAsync(currentUser as IPermissionedUser, PermissionsCodes.MENU_ITEMS_REGISTRY))
                {
                    return response.SetError(T("Autorizzazioni non sufficenti per creare/modificare una voce di menu"), 2305191149, logger);
                }


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
                    model.AuthorizedRoles.UpdateLists(roles, r => r.Code);
                }

                // *RequiredPermissions
                if (dto.RequiredPermissionsCode != null && dto.RequiredPermissionsCode.Any())
                {
                    var permissions = await unitOfWork.Session.QueryOver<IPermissionModel>().Where(x => x.Code.IsIn(dto.RequiredPermissionsCode)).ListAsync();
                    model.RequiredPermissions ??= new List<IPermissionModel>();
                    model.RequiredPermissions.UpdateLists(permissions, p => p.Code);
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

        public async Task<IApiResponse<IMenuItemViewModel[]?>> GetMenuItemsByMenuCodeAsync(string menuCode, IUserModel currentUser)
        {
            return await ExecuteTransactionAsync((Func<IApiResponse<IMenuItemViewModel[]?>, Task<IApiResponse<IMenuItemViewModel[]?>>>)(async (response) =>
            {
                IMenuItemModel menuItem = null;
                IMenuModel parentMenu = null;
                IPermissionModel permission = null;
                var menuItems = await unitOfWork.Session.QueryOver<IMenuItemModel>(() => menuItem)
                 .JoinAlias(x => x.ParentMenu, () => parentMenu, JoinType.LeftOuterJoin)
                 .JoinAlias(x => x.RequiredPermissions, () => permission, JoinType.LeftOuterJoin)
                 .Fetch(SelectMode.Fetch, x => x.RequiredPermissions)
                 .Where(() => parentMenu.Code == menuCode && menuItem.ParentItem == null).ListAsync();

                var query = menuItems
                .Where(m => m.IsEnabled).ToList();

                if (!currentUser.IsAdmin())
                {
                    if (currentUser is IPermissionedUser permissionedUser)
                        RemoveForbiddenMenuItems(query, permissionedUser);

                    if (currentUser is IRoledUser roledUser)
                    {
                        RemoveUnAuthorizedMenuItems(query, roledUser);
                    }
                }


                response.Value = mapper.Map<IMenuItemViewModel[]>(query);

                return response;

            }), "Errore ottenendo l'elenco delle voci di menu");
        }

        private static void RemoveForbiddenMenuItems(ICollection<IMenuItemModel> query, IPermissionedUser permissionedUser)
        {
            var currentUserPermissionsCodes = permissionedUser.UsersPermissions.Where(up => up.Permission.Enabled).Select(up => up.Permission.Code);

            foreach (IMenuItemModel menuItem in query.ToList())
            {
                var menuItemRequiredPermissions = menuItem.RequiredPermissions.Where(rp => rp.Enabled).Select(rp => rp.Code);
                var notSatisfiedPErmissions = menuItem.RequiredPermissions.Where(rp => !currentUserPermissionsCodes.Any(code => menuItemRequiredPermissions.Contains(code))).Where(n => n.Enabled);

                if (notSatisfiedPErmissions.Any())
                {
                    // not permissions enough ... remove the menu item
                    query.Remove(menuItem);
                }
                else
                {
                    // Check if this item has nested sub items
                    if (menuItem.IsTreeView == true && menuItem.SubItems != null && menuItem.SubItems.Any())
                    {
                        RemoveForbiddenMenuItems(menuItem.SubItems, permissionedUser);
                    }
                }
            }
        }

        private static void RemoveUnAuthorizedMenuItems(ICollection<IMenuItemModel> query, IRoledUser roledUser)
        {
            var currentUserRolesCodes = roledUser.Roles.Where(r => r.Enabled).Select(up => up.Code);

            foreach (IMenuItemModel menuItem in query.ToList())
            {
                var menuItemRequiredRoles = menuItem.AuthorizedRoles.Where(r => r.Enabled).Select(r => r.Code);

                if (!menuItemRequiredRoles.Any(m => currentUserRolesCodes.Contains(m)))
                {
                    // not member of any role ... remove the menu item
                    query.Remove(menuItem);
                }
                else
                {
                    // Check if this item has nested sub items
                    if (menuItem.IsTreeView == true && menuItem.SubItems != null && menuItem.SubItems.Any())
                    {
                        RemoveUnAuthorizedMenuItems(menuItem.SubItems, roledUser);
                    }
                }
            }
        }
        public async Task<IApiResponse> InitServiceAsync()
        {
            return new ApiResponse();
            //throw new NotImplementedException();
            //TODO: Implementa
        }
    }
}