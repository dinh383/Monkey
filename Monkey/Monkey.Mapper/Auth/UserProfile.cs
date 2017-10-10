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
using Monkey.Core.Constants.Auth;
using Monkey.Core.Entities.User;
using Monkey.Core.Models.Auth;
using Puppy.AutoMapper;
using Puppy.Core.StringUtils;

namespace Monkey.Mapper.Auth
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, LoggedInUserModel>().IgnoreAllNonExisting()
                .ForMember(d => d.Subject, o => o.MapFrom(s => s.GlobalId));

            CreateMap<ProfileEntity, LoggedInUserModel>().IgnoreAllNonExisting();

            CreateMap<SignInModel, RequestTokenModel>().IgnoreAllNonExisting()
                .ForMember(d => d.GrantType, o => o.UseValue(GrantType.Password));

            CreateMap<UserCreateModel, UserEntity>().IgnoreAllNonExisting();

            CreateMap<UserUpdateModel, UserEntity>().IgnoreAllNonExisting()
                .ForMember(d => d.UserNameNorm, o => o.MapFrom(s => StringHelper.Normalize(s.UserName)));

            CreateMap<UserEntity, UserModel>().IgnoreAllNonExisting()
                .ForMember(d => d.Subject, o => o.MapFrom(s => s.GlobalId))
                .ForMember(d => d.IsBanned, o => o.MapFrom(s => s.BannedTime != null))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.Profile.FullName))
                .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role != null ? s.Role.Name : string.Empty));

            CreateMap<UserModel, UserUpdateModel>().IgnoreAllNonExisting();
        }
    }
}