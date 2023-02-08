using bs.Frmwrk.Core.Models.Auth;

namespace bs.Frmwrk.Core.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public interface ISecurityRepository
    {
        /// <summary>
        /// Creates the permission asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task CreatePermissionAsync(IPermissionModel model);

        /// <summary>
        /// Gets the user by user name asynchronous.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        Task<IUserModel> GetUserByUserNameAsync(string userName);

        /// <summary>
        /// Updates the permission asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task UpdatePermissionAsync(IPermissionModel model);
    }
}