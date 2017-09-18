#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Business Interface </Project>
//     <File>
//         <Name> IClientBusiness.cs </Name>
//         <Created> 14/09/17 8:18:33 PM </Created>
//         <Key> cb340be9-ae66-469b-a702-bf2c7c754c8d </Key>
//     </File>
//     <Summary>
//         IClientBusiness.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Models.Auth;
using System.Threading.Tasks;

namespace Monkey.Business
{
    public interface IClientBusiness : IBaseBusiness
    {
        Task<int> GetTotalAsync();

        Task<ClientModel> CreateAsync(ClientCreateModel model);

        void CheckExistByName(params string[] names);

        Task<int> GetIdAsync(string globalId, string secret);

        void CheckExist(string subject, string secret);

        void CheckBanned(string subject, string secret);
    }
}