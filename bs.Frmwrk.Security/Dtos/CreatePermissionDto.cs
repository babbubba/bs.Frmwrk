using bs.Frmwrk.Core.Dtos.Security;

namespace bs.Frmwrk.Security.Dtos
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="bs.Frmwrk.Core.Dtos.Security.ICreatePermissionDto" />
    public class CreatePermissionDto : ICreatePermissionDto
    {
        public CreatePermissionDto(string code, string name)
        {
            Code = code;
            Label = name;
        }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:bs.Frmwrk.Core.Models.Auth.IRoleModel" /> is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get; set; }


        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Label { get; set; }
    }
}