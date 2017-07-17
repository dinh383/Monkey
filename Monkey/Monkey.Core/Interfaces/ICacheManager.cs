#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Interface </Project>
//     <File>
//         <Name> ICacheManager.cs </Name>
//         <Created> 17/07/17 5:18:02 PM </Created>
//         <Key> 6ba8510d-fae1-4818-ace2-a6eea4e17cba </Key>
//     </File>
//     <Summary>
//         ICacheManager.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Puppy.Core.CacheUtils;

namespace Monkey.Core.Interfaces
{
    public interface ICacheManager : IDistributedCacheHelper
    {
    }
}