#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> ServiceCollectionExtensions.cs </Name>
//         <Created> 22 Apr 17 8:02:59 PM </Created>
//         <Key> 50e7f986-c196-4507-accf-e02189d15c7d </Key>
//     </File>
//     <Summary>
//         ServiceCollectionExtensions.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using Monkey.Mapper.Profiles;
using Microsoft.Extensions.DependencyInjection;
using Puppy.AutoMapper;

namespace Monkey.Mapper
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapperMonkey(this IServiceCollection services)
        {
            services.AddAutoMapper(isAssertConfigurationIsValid: true, isCompileMappings: true, profileAssemblyMarkerTypes: typeof(IAutoMapperProfile));

            return services;
        }
    }
}