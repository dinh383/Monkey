﻿#region	License
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

using Monkey.Model.Models.Client;
using System.Threading.Tasks;

namespace Monkey.Business
{
    public interface IClientBusiness : IBaseBusiness
    {
        Task<int> GetIdAsync(string globalId, string secret);

        void CheckExist(string globalId, string secret);

        void CheckExists(params string[] globalIds);

        void CheckExist(params string[] names);

        Task<int> GetTotalAsync();

        Task<ClientModel> CreateAsync(ClientCreateModel model);
    }
}