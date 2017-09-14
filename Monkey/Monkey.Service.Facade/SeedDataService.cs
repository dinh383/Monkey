#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Service Facade </Project>
//     <File>
//         <Name> SeedDataService.cs </Name>
//         <Created> 14/09/17 11:21:24 AM </Created>
//         <Key> ffec5eb8-f942-4a6d-b157-55b7e11dec23 </Key>
//     </File>
//     <Summary>
//         SeedDataService.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Puppy.DependencyInjection.Attributes;

namespace Monkey.Service.Facade
{
    [PerRequestDependency(ServiceType = typeof(ISeedDataService))]
    public class SeedDataService : ISeedDataService
    {
        public SeedDataService()
        {
        }
    }
}