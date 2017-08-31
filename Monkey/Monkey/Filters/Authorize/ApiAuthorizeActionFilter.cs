using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Monkey.Core;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Monkey.Filters.Authorize
{
    public class ApiAuthorizeActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if (controllerActionDescriptor == null) return;

            // If allow anonymous => the user/request is authorized
            if (IsAllowAnonymous(controllerActionDescriptor))
                return;

            // Check is user pass authentication
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var actionAttributes = controllerActionDescriptor.MethodInfo.GetCustomAttributes<AuthorizeAttribute>(true).ToList();
            var listAllowPermission = actionAttributes.SelectMany(x => x.Permissions).ToList();

            var controllerAttributes = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes<AuthorizeAttribute>(true);
            var listControllerPermission = controllerAttributes.SelectMany(x => x.Permissions).ToList();

            // If Action combine Controller authorize or Action not have any Authorize Attribute,
            // then add allow permission of controller to the list allow permission.
            bool isCombineAuthorize = controllerActionDescriptor.MethodInfo.GetCustomAttributes<CombineAuthorizeAttribute>(true).Any();

            if (isCombineAuthorize || !actionAttributes.Any())
            {
                listAllowPermission.AddRange(listControllerPermission);
            }

            // If list allow permission don't have any thing => User is authorized
            if (!listAllowPermission.Any()) return;

            List<Enums.Permission> listUserPermission = GetUserListPermission(context.HttpContext);

            // If user have any permission in list allow permission => the user/request is authorized

            if (listAllowPermission.Any(x => listUserPermission.Contains(x))) return;

            context.Result = new UnauthorizedResult();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Nothing to filter in Action Executed
        }

        private static bool IsAllowAnonymous(ControllerActionDescriptor controllerActionDescriptor)
        {
            // If action have any AllowAnonymousAttribute => Allow Anonymous
            bool isActionAllowAnonymous = controllerActionDescriptor.MethodInfo.GetCustomAttributes<AllowAnonymousAttribute>(true).Any();
            if (isActionAllowAnonymous) return true;

            var isActionHaveAnyPermission = controllerActionDescriptor.MethodInfo.GetCustomAttributes<AuthorizeAttribute>(true).Any();

            bool isCombineAuthorize = controllerActionDescriptor.MethodInfo.GetCustomAttributes<CombineAuthorizeAttribute>(true).Any();

            if (!isCombineAuthorize && isActionHaveAnyPermission) return false;

            bool isControllerAllowAnonymous = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes<AllowAnonymousAttribute>(true).Any();
            if (isControllerAllowAnonymous) return true;

            if (!isActionHaveAnyPermission)
            {
                var isControllerHaveAnyPermission = controllerActionDescriptor.MethodInfo.GetCustomAttributes<AuthorizeAttribute>(true).Any();
                return !isControllerHaveAnyPermission;
            }

            return false;
        }

        private static List<Enums.Permission> GetUserListPermission(HttpContext context)
        {
            // TODO get list permission from http context/request by JWT
            return new List<Enums.Permission>();
        }
    }
}