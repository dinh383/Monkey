#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> MapperConfiguration.cs </Name>
//         <Created> 22 Apr 17 8:02:59 PM </Created>
//         <Key> 50e7f986-c196-4507-accf-e02189d15c7d </Key>
//     </File>
//     <Summary>
//         MapperConfiguration.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Monkey.Mapper
{
    public class MapperConfiguration
    {
        public static void Service(IServiceCollection services)
        {
            // Add Auto Mapper
            services.AddAutoMapper();

            // Add Config for Auto Mapper
            Configure();
        }

        public static void Configure()
        {
            // Add Auto Mapper Profiles
            AutoMapper.Mapper.Initialize(x =>
            {
            });

            // Check all auto mapper profile is valid
            AutoMapper.Mapper.AssertConfigurationIsValid();

            // AutoMapper lazily compiles the type map plans on first map. However, this behavior is
            // not always desirable, so you can tell AutoMapper to compile its mappings directly
            AutoMapper.Mapper.Configuration.CompileMappings();
        }
    }
}