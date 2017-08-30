using System;

namespace Monkey.Filters.Authorize
{
    /// <summary>
    ///     Attribute to mark clear higher level AuthorizeAttribute setup 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class OverrideAuthorizeAttribute : Attribute
    {
    }
}