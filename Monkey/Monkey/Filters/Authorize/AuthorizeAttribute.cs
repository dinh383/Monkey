using Microsoft.AspNetCore.Mvc;
using Monkey.Core;
using System;

namespace Monkey.Filters.Authorize
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public Enums.Permission[] Permissions { get; }

        public AuthorizeAttribute(params Enums.Permission[] permissions) : base(typeof(AuthorizeActionFilter))
        {
            Permissions = permissions;
        }
    }
}