using bs.Frmwrk.Core.Dtos.Auth;

namespace bs.Frmwrk.Auth.Dtos
{
    public class CreateRoleDto : ICreateRoleDto
    {
        public CreateRoleDto(string code, string label)
        {
            Code = code;
            Label = label;
        }

        public CreateRoleDto(string code, string label, string[] permissionsCode)
        {
            Code = code;
            Label = label;
            PermissionsCode = permissionsCode;
        }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the permissions code that are associated to the role.
        /// </summary>
        /// <value>
        /// The permissions code.
        /// </value>
        public string[]? PermissionsCode { get; set; }
    }
}