using System;

namespace Monkey.Filters.Authorize
{
    /// <summary>
    ///     Attribute to mark combine (OR conditional) with higher level AuthorizeAttribute 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CombineAuthorizeAttribute : Attribute
    {
    }
}