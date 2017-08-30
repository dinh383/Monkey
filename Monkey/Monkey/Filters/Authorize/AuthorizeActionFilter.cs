using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Monkey.Core;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Monkey.Filters.Authorize
{
    public class AuthorizeActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if (controllerActionDescriptor == null)
            {
                await next().ConfigureAwait(true);
                return;
            }

            // If allow anonymous => the user/request is authorized
            if (IsAllowAnonymous(controllerActionDescriptor))
            {
                await next().ConfigureAwait(true);
                return;
            }

            List<Enums.Permission> listUserPermission = GetUserListPermission(context.HttpContext);

            var actionAttributes = controllerActionDescriptor.MethodInfo.GetCustomAttributes<AuthorizeAttribute>(true);
            var listAllowPermission = actionAttributes.SelectMany(x => x.Permissions).ToList();

            // If Action not Override/Clear Controller authorize, then add allow permission of
            // controller to the list allow permission.
            bool isClearAuthorizeController = controllerActionDescriptor.MethodInfo.GetCustomAttributes<OverrideAuthorizeAttribute>(true).Any();
            if (!isClearAuthorizeController)
            {
                var controllerAttributes = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes<AuthorizeAttribute>(true);
                var listControllerPermission = controllerAttributes.SelectMany(x => x.Permissions).ToList();

                listAllowPermission.AddRange(listControllerPermission);
            }

            // If user have any permission in list allow permission => the user/request is authorized
            if (listAllowPermission.Any(x => listUserPermission.Contains(x)))
            {
                await next().ConfigureAwait(true);
            }
            else
            {
                context.Result = new UnauthorizedResult();
                await context.Result.ExecuteResultAsync(context).ConfigureAwait(true);
            }
        }

        private static bool IsAllowAnonymous(ControllerActionDescriptor controllerActionDescriptor)
        {
            // If action have any AllowAnonymousAttribute => Allow Anonymous
            bool isActionAllowAnonymous = controllerActionDescriptor.MethodInfo.GetCustomAttributes<AllowAnonymousAttribute>(true).Any();
            if (isActionAllowAnonymous) return true;

            // If action not have any AllowAnonymousAttribute, but they have any AuthorizeAttribute
            // => Not Allow Anonymous, use action Authorization
            var isActionHavePermission = controllerActionDescriptor.MethodInfo.GetCustomAttributes<AuthorizeAttribute>(true).Any();
            if (isActionHavePermission) return false;

            // If action not have any AllowAnonymousAttribute and AuthorizeAttribute, then depend on
            // controller. If controller have any AllowAnonymousAttribute => Allow Anonymous
            bool isAllowControllerAnonymous = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes<AllowAnonymousAttribute>(true).Any();
            return isAllowControllerAnonymous;
        }

        private static List<Enums.Permission> GetUserListPermission(HttpContext context)
        {
            // TODO get list permission from http context/request by JWT
            return new List<Enums.Permission>();
        }
    }
}