#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> ValidatorServiceExtensions.cs </Name>
//         <Created> 17/09/17 11:36:50 PM </Created>
//         <Key> 57e0342c-c0b3-4ebe-9725-0639880d7230 </Key>
//     </File>
//     <Summary>
//         ValidatorServiceExtensions.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Monkey.Core.Validators
{
    public static class ValidatorMvcBuilderExtensions
    {
        public static IMvcBuilder AddValidator(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddViewOptions(options =>
                {
                    // Enable Microsoft.jQuery.Unobtrusive.Validation
                    options.HtmlHelperOptions.ClientValidationEnabled = true;
                })
                .AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<IModelValidator>());

            return mvcBuilder;
        }
    }
}