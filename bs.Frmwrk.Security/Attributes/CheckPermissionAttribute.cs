//using bs.Data.Interfaces;
//using bs.Frmwrk.Core.Exceptions;
//using bs.Frmwrk.Core.Models.Auth;
//using bs.Frmwrk.Core.Services.Security;
//using bs.Frmwrk.Shared;
//using Microsoft.AspNetCore.Http;
//using NHibernate.Id.Insert;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;

//namespace bs.Frmwrk.Security.Attributes
//{
//    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Event, AllowMultiple = true)]
//    sealed public class CheckPermissionAttribute : ValidationAttribute
//    {
//        private readonly string permissionCode;
//        private readonly PermissionType permissionType;

//        public CheckPermissionAttribute(string permissionCode, PermissionType permissionType)
//        {
//            this.permissionCode = permissionCode;
//            this.permissionType = permissionType;
//        }
//        public override bool IsValid(object? value)
//        {
//            return base.IsValid(value);
//        }

//        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
//        {
//            var securityService = (ISecurityService?)validationContext.GetService(typeof(ISecurityService));
//            var httpContextAccessor = (IHttpContextAccessor?)validationContext.GetService(typeof(IHttpContextAccessor));
//            var uow = (IUnitOfWork?)validationContext.GetService(typeof(IUnitOfWork));
//            if (uow is null)
//            {
//                throw new BsException(2301090947, "Cannot resolve the 'Unit of work'");
//            }
//            if (securityService is null)
//            {
//                throw new BsException(2301090948, "Cannot resolve the 'Security service'");
//            }
//            if (httpContextAccessor is null)
//            {
//                throw new BsException(2301090949, "Cannot resolve the 'Http context'");
//            }

//            var currentUserClaim = httpContextAccessor.HttpContext?.User;

//            if (currentUserClaim?.Identity is not null && currentUserClaim.Identity.IsAuthenticated)
//            {
//                var userIdValue = currentUserClaim.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//                var currentUser = (IPermissionedUser)uow.Session.Get<IUserModel>(userIdValue.ToGuid());
//                var isValid = securityService.CheckUserPermissionAsync(currentUser, permissionCode, permissionType).Result;
//                if (isValid)
//                {
//                    return ValidationResult.Success;
//                }
//                else
//                {
//                    return new ValidationResult($"User has not the valid permission for this action");
//                }
//            }

//            return new ValidationResult($"User is not autenticated and cannot be validate");
//        }
//    }

//}