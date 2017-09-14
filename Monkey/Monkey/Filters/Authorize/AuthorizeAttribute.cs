﻿using Monkey.Core;
using System;
using Monkey.Core.Constants;

namespace Monkey.Filters.Authorize
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizeAttribute : Attribute
    {
        public Enums.Permission[] Permissions { get; }

        public AuthorizeAttribute(params Enums.Permission[] permissions)
        {
            Permissions = permissions;
        }
    }
}