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
using Monkey.Core.Models.User;
using Puppy.AutoMapper;
using Puppy.Core.StringUtils;
using System.Linq;

namespace Monkey.Mapper.User
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<SignInModel, RequestTokenModel>()
                .IgnoreAllNonExisting()
                .ForMember(d => d.GrantType, o => o.UseValue(GrantType.Password));

            CreateMap<UserCreateModel, UserEntity>().IgnoreAllNonExisting()
                .ForMember(d => d.EmailNorm, o => o.MapFrom(s => StringHelper.Normalize(s.Email)))
                .ForMember(d => d.UserNameNorm, o => o.MapFrom(s => StringHelper.Normalize(s.UserName)))
                ;

            CreateMap<UserCreateModel, ProfileEntity>()
                .IgnoreAllNonExisting()
                .ForMember(d => d.FullNameNorm, o => o.MapFrom(s => StringHelper.Normalize(s.FullName)));

            CreateMap<UserUpdateModel, UserEntity>()
                .IgnoreAllNonExisting()
                .ForMember(d => d.EmailNorm, o => o.MapFrom(s => StringHelper.Normalize(s.Email)))
                .ForMember(d => d.UserNameNorm, o => o.MapFrom(s => StringHelper.Normalize(s.UserName)));

            CreateMap<UserUpdateModel, ProfileEntity>()
                .IgnoreAllNonExisting()
                .ForMember(d => d.FullNameNorm, o => o.MapFrom(s => StringHelper.Normalize(s.FullName)));

            CreateMap<UserEntity, UserModel>()
                .IgnoreAllNonExisting()
                .ForMember(d => d.Subject, o => o.MapFrom(s => s.GlobalId))
                .ForMember(d => d.IsBanned, o => o.MapFrom(s => s.BannedTime != null))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.Profile.FullName))
                .ForMember(d => d.AvatarId, o => o.MapFrom(s => s.Profile.AvatarId))
                .ForMember(d => d.AvatarUrl, o => o.MapFrom(s => s.Profile.Avatar.Url))
                .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role != null ? s.Role.Name : string.Empty));

            CreateMap<UserEntity, LoggedInUserModel>()
                .IgnoreAllNonExisting()
                .ForMember(d => d.Subject, o => o.MapFrom(s => s.GlobalId))
                .ForMember(d => d.IsBanned, o => o.MapFrom(s => s.BannedTime != null))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.Profile.FullName))
                .ForMember(d => d.AvatarId, o => o.MapFrom(s => s.Profile.AvatarId))
                .ForMember(d => d.AvatarUrl, o => o.MapFrom(s => s.Profile.Avatar.Url))
                .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role != null ? s.Role.Name : string.Empty))

                // More information than UserModel
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.Profile.FullName))
                .ForMember(d => d.ListPermission, o => o.MapFrom(s => s.Role.Permissions.Select(y => y.Permission)));

            CreateMap<UserModel, UserUpdateModel>()
                .IgnoreAllNonExisting();

            CreateMap<UserEntity, CreateUserResultModel>()
                .IgnoreAllNonExisting()
                .ForMember(d => d.Subject, o => o.MapFrom(s => s.GlobalId));

            CreateMap<LoggedInUserModel, UpdateProfileModel>()
                .IgnoreAllNonExisting();

            CreateMap<UserEntity, UserLookupModel>()
                .IgnoreAllNonExisting()
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.Profile.FullName));
        }
    }
}