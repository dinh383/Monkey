#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> ModelValidatorExtensions.cs </Name>
//         <Created> 17/09/17 11:36:50 PM </Created>
//         <Key> 57e0342c-c0b3-4ebe-9725-0639880d7230 </Key>
//     </File>
//     <Summary>
//         ModelValidatorExtensions.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Monkey.Core.Validators
{
    public static class ModelValidatorExtensions
    {
        /// <summary>
        ///     [Validator] Model Validator, Must after "AddMvc" 
        /// </summary>
        /// <param name="mvcBuilder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddModelValidator(this IMvcBuilder mvcBuilder)
        {
            // Enable Microsoft.jQuery.Unobtrusive.Validation for Front-end
            mvcBuilder.AddViewOptions(options =>
            {
                options.HtmlHelperOptions.ClientValidationEnabled = true;
            });

            // Register Fluent Validation Rules
            mvcBuilder.AddFluentValidation(fvc =>
            {
                fvc.RegisterValidatorsFromAssemblyContaining<IModelValidator>();
            });

            return mvcBuilder;
        }
    }
}