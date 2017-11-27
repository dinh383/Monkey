#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Service Interface </Project>
//     <File>
//         <Name> IInitialService.cs </Name>
//         <Created> 14/09/17 11:21:10 AM </Created>
//         <Key> 601cb8d5-f839-4a9f-b12d-300d6fb67601 </Key>
//     </File>
//     <Summary>
//         IInitialService.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

namespace Monkey.Service
{
    public interface IInitialService : IBaseService
    {
        void InitialData();

        void ReBuildCache();
    }
}