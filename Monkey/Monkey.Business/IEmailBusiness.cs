#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Business Interface </Project>
//     <File>
//         <Name> IEmailBusiness.cs </Name>
//         <Created> 09/10/17 8:34:29 PM </Created>
//         <Key> c70155aa-4efd-4a96-ac31-5fed3b5b6ef6 </Key>
//     </File>
//     <Summary>
//         IEmailBusiness.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using System;

namespace Monkey.Business
{
    public interface IEmailBusiness : IBaseBusiness
    {
        void SendActiveAccount(string token, string email, TimeSpan expireIn);

        void SendSetPassword(string token, string email, TimeSpan expireIn);
    }
}