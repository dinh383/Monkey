#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Mapper <Project>
//     <File>
//         <Name> LoggedUserModelProfile.cs </Name>
//         <Created> 14/09/17 7:42:25 PM </Created>
//         <Key> 85c5d0cd-6bbb-4478-9cba-bc859dcd29a9 </Key>
//     </File>
//     <Summary>
//         LoggedUserModelProfile.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using AutoMapper;
using Monkey.Data.Entities.User;
using Monkey.Model.Models.User;
using Puppy.AutoMapper;

namespace Monkey.Mapper.Profiles
{
    public class LoggedUserModelProfile : Profile
    {
        public LoggedUserModelProfile()
        {
            CreateMap<UserEntity, LoggedUserModel>()
                .IgnoreAllNonExisting();
        }
    }
}