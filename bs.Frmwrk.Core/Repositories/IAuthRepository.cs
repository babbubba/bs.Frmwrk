using bs.Frmwrk.Core.Models.Auth;

namespace bs.Frmwrk.Core.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public interface IAuthRepository
    {
        ///// <summary>
        ///// Gets the user by user name asynchronous.
        ///// </summary>
        ///// <param name="userName">Name of the user.</param>
        ///// <returns></returns>
        //Task<IUserModel> GetUserByUserNameAsync(string userName);

        /// <summary>
        /// Gets the user by identifier asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<IUserModel> GetUserByIdAsync(Guid userId);

        /// <summary>
        /// Gets the role by identifier asynchronous.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        Task<IRoleModel> GetRoleByIdAsync(Guid roleId);
    }
}