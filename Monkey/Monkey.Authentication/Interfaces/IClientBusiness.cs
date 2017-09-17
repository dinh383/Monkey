#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Interface </Project>
//     <File>
//         <Name> IClientBusiness.cs </Name>
//         <Created> 17/09/17 3:04:27 PM </Created>
//         <Key> dbd46799-68a5-47da-a770-5a31c4508da0 </Key>
//     </File>
//     <Summary>
//         IClientBusiness.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using System.Threading.Tasks;

namespace Monkey.Authentication.Interfaces
{
    public interface IClientBusiness
    {
        Task<int> GetIdAsync(string subject, string secret);

        void CheckExist(string subject, string secret);
    }
}