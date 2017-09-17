#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Interface </Project>
//     <File>
//         <Name> IClientBusiness.cs </Name>
//         <Created> 17/09/17 11:56:27 PM </Created>
//         <Key> 5b3a51e5-a4d2-41d3-a19d-019a86972c36 </Key>
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
        Task<int> GetIdAsync(string globalId, string secret);

        void CheckExist(string subject, string secret);

        void CheckBanned(string subject, string secret);
    }
}