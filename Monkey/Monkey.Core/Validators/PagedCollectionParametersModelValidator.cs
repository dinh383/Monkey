#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> PagedCollectionParametersModelValidator.cs </Name>
//         <Created> 27/08/17 12:58:58 AM </Created>
//         <Key> eae63f22-40f6-4b07-ae16-4cf33de1b54b </Key>
//     </File>
//     <Summary>
//         PagedCollectionParametersModelValidator.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Models;
using FluentValidation;

namespace Monkey.Core.Validators
{
    public class PagedCollectionParametersModelValidator : AbstractValidator<PagedCollectionParametersModel>
    {
        public PagedCollectionParametersModelValidator()
        {
            RuleFor(reg => reg.Skip).GreaterThanOrEqualTo(0);
            RuleFor(reg => reg.Take).LessThanOrEqualTo(10000);
        }
    }
}