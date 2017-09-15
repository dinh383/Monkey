#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> RequestTokenModelValidator.cs </Name>
//         <Created> 27/08/17 12:58:58 AM </Created>
//         <Key> eae63f22-40f6-4b07-ae16-4cf33de1b54b </Key>
//     </File>
//     <Summary>
//         RequestTokenModelValidator.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using FluentValidation;
using Monkey.Core.Models.User;

namespace Monkey.Core.Validators.User
{
    public class RequestTokenModelValidator : AbstractValidator<RequestTokenModel>
    {
        public RequestTokenModelValidator()
        {
        }
    }
}