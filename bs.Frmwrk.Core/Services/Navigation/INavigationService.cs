using bs.Frmwrk.Core.Dtos.Navigation;
using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.ViewModels.Api;
using bs.Frmwrk.Core.ViewModels.Navigation;

namespace bs.Frmwrk.Core.Services.Navigation
{
    /// <summary>
    ///
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Creates the menu if not exists otherwise upodate it asynchronous.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="currentUser">The current user.</param>
        /// <returns></returns>
        Task<IApiResponse<IMenuViewModel>> CreateUpdateMenuAsync(ICreateMenuDto menu, IUserModel currentUser);

        /// <summary>
        /// Creates the menu item if not exists otherwise update it asynchronous.
        /// </summary>
        /// <param name="menuItem">The menu item.</param>
        /// <param name="currentUser">The current user.</param>
        /// <returns></returns>
        Task<IApiResponse<IMenuItemViewModel>> CreateUpdateMenuItemAsync(ICreateMenuItemDto menuItem, IUserModel currentUser);

        /// <summary>
        /// Deletes the menu asynchronous.
        /// </summary>
        /// <param name="menuItemId">The menu item identifier.</param>
        /// <param name="currentUser">The current user.</param>
        /// <returns></returns>
        Task<IApiResponse> DeleteMenuAsync(string menuItemId, IUserModel currentUser);

        /// <summary>
        /// Deletes the menu item asynchronous.
        /// </summary>
        /// <param name="menuItemId">The menu item identifier.</param>
        /// <param name="currentUser">The current user.</param>
        /// <returns></returns>
        Task<IApiResponse> DeleteMenuItemAsync(string menuItemId, IUserModel currentUser);

        /// <summary>
        /// Gets the menu by code (if exists) asynchronous.
        /// </summary>
        /// <param name="menuCode">The menu code.</param>
        /// <param name="currentUser">The current user.</param>
        /// <returns></returns>
        Task<IApiResponse<IMenuViewModel?>> GetMenuByCodeAsync(string menuCode, IUserModel currentUser);

        /// <summary>
        /// Gets the menu item by code (if exists) asynchronous.
        /// </summary>
        /// <param name="menuItemCode">The menu item code.</param>
        /// <param name="currentUser">The current user.</param>
        /// <returns></returns>
        Task<IApiResponse<IMenuItemViewModel?>> GetMenuItemByCodeAsync(string menuItemCode, IUserModel currentUser);

        /// <summary>
        /// Gets the menu items by menu code (if exists) asynchronous.
        /// </summary>
        /// <param name="menuCode">The menu code.</param>
        /// <param name="currentUser">The current user.</param>
        /// <returns></returns>
        Task<IApiResponse<IMenuItemViewModel[]?>> GetMenuItemsByMenuCodeAsync(string menuCode, IUserModel currentUser);
        Task<IApiResponse<IMenuViewModel>> _CreateUpdateMenuAsync(ICreateMenuDto menu, IUserModel currentUser);
        Task<IApiResponse<IMenuItemViewModel>> _CreateUpdateMenuItemAsync(ICreateMenuItemDto menuItem, IUserModel currentUser);
    }
}