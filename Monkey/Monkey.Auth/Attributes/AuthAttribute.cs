using Monkey.Core.Constants;
using System;

namespace Monkey.Auth.Filters.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthAttribute : Attribute
    {
        public Enums.Permission[] Permissions { get; }

        public AuthAttribute(params Enums.Permission[] permissions)
        {
            Permissions = permissions;
        }
    }
}