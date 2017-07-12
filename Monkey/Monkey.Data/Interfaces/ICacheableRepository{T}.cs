#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Interface </Project>
//     <File>
//         <Name> ICacheableRepository.cs </Name>
//         <Created> 21/05/2017 6:49:21 PM </Created>
//         <Key> 80aa509d-8129-4f3e-9623-4c3c6e6bc2d2 </Key>
//     </File>
//     <Summary>
//         ICacheableRepository.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using System;
using System.Collections.Generic;

namespace Monkey.Data.Interfaces
{
    public interface ICacheableRepository<T> where T : class
    {
        List<T> GetCache(Func<T, bool> predicate = null);

        List<T> ReInitialCache();

        void RemoveCache();
    }
}