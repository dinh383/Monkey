#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Service Interface </Project>
//     <File>
//         <Name> IClientService.cs </Name>
//         <Created> 02/10/17 3:48:36 PM </Created>
//         <Key> 4b604e36-7c6f-489a-9242-c9e466d595ec </Key>
//     </File>
//     <Summary>
//         IClientService.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using System.Threading.Tasks;
using Monkey.Core.Models.Auth;

namespace Monkey.Service
{
    public interface IClientService : IBaseService
    {
        Task<ClientCreatedModel> CreateAsync(ClientCreateModel model);

        Task<string> GenerateSecretAsync(int id);
    }
}