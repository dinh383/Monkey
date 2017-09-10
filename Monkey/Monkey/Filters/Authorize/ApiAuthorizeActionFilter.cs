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
            if (!(context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)) return;

            // If allow anonymous => the user/request is authorized
            if (IsAllowAnonymous(controllerActionDescriptor))
                return;

            // Check is user pass authentication
            if (!IsAuthenticated(context.HttpContext))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            List<AuthorizeAttribute> listAuthorizeAttribute = new List<AuthorizeAttribute>();
            var actionAttributes = controllerActionDescriptor.MethodInfo.GetCustomAttributes<AuthorizeAttribute>(true).ToList();
            if (actionAttributes.Any())
            {
                listAuthorizeAttribute.AddRange(actionAttributes);
            }

            // If Action combine Controller authorize or Action not have any Authorize Attribute,
            // then add allow permission of controller to the list allow permission.
            bool isCombineAuthorize = controllerActionDescriptor.MethodInfo.GetCustomAttributes<CombineAuthorizeAttribute>(true).Any();

            if (isCombineAuthorize || !listAuthorizeAttribute.Any())
            {
                var controllerAttributes = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes<AuthorizeAttribute>(true);
                listAuthorizeAttribute.AddRange(controllerAttributes);
            }

            // If list attribute or list allow permission don't have any thing => User is authorized
            if (!listAuthorizeAttribute.Any() || listAuthorizeAttribute.SelectMany(x => x.Permissions).Any() != true) return;

            // Get current user logged in permission
            List<Enums.Permission> listUserPermission = GetUserListPermission(context.HttpContext);

            // Apply rule AND conditional for list attribute, OR conditional for permission into an attribute

            // Only check attribute have permission
            listAuthorizeAttribute = listAuthorizeAttribute.Where(x => x.Permissions?.Any() == true).ToList();

            bool isUserAuthorized = listAuthorizeAttribute.All(x => x.Permissions.Any(y => listUserPermission.Contains(y)));

            if (isUserAuthorized)
            {
                return;
            }

            // User not match conditional, so un-authorization
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
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
                var isControllerHaveAnyPermission = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes<AuthorizeAttribute>(true).Any();
                return !isControllerHaveAnyPermission;
            }

            return false;
        }

        private static bool IsAuthenticated(HttpContext context)
        {
            return context.User.Identity.IsAuthenticated;
        }

        private static List<Enums.Permission> GetUserListPermission(HttpContext context)
        {
            // TODO get list permission from http context/request by JWT
            return new List<Enums.Permission>();
        }
    }
}