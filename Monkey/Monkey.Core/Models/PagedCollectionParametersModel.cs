#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> PagedCollectionParametersModel.cs </Name>
//         <Created> 27/08/17 12:58:35 AM </Created>
//         <Key> ab233b33-d279-45f1-b326-73ff8b7c627e </Key>
//     </File>
//     <Summary>
//         PagedCollectionParametersModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using FluentValidation.Attributes;
using Monkey.Core.Validators;

namespace Monkey.Core.Models
{
    [Validator(typeof(PagedCollectionParametersModelValidator))]
    public class PagedCollectionParametersModel
    {
        public int Skip { get; set; } = SystemConfigs.PagedCollectionParameters.Skip;

        public int Take { get; set; } = SystemConfigs.PagedCollectionParameters.Take;

        public string Terms { get; set; } = SystemConfigs.PagedCollectionParameters.Terms;
    }
}