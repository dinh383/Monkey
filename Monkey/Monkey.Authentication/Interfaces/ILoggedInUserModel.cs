#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Interface </Project>
//     <File>
//         <Name> ILoggedInUserModel.cs </Name>
//         <Created> 18/09/17 12:10:44 AM </Created>
//         <Key> 8f081371-3228-411c-861d-07ff0cc0bf04 </Key>
//     </File>
//     <Summary>
//         ILoggedInUserModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

namespace Monkey.Authentication.Interfaces
{
    public interface ILoggedInUserModel
    {
        /// <summary>
        ///     Client Id in Access Token, this info show user logged in via what client 
        /// </summary>
        string ClientSubject { get; set; }

        string Subject { get; set; }

        int Id { get; set; }
    }
}