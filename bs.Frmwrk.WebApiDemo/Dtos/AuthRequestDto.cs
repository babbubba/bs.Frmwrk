using bs.Frmwrk.Core.Dtos.Auth;

namespace bs.Frmwrk.WebApiDemo.Dtos
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="bs.Frmwrk.Core.Dtos.Auth.IAuthRequestDto" />
    public class AuthRequestDto : IAuthRequestDto
    {
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        /// <value>
        /// The user name.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }
    }
}