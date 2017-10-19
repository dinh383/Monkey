using AutoMapper;
using Monkey.Core.Entities.Auth;
using Monkey.Core.Models.Auth;
using Puppy.AutoMapper;
using System.Linq;

namespace Monkey.Mapper.Auth
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleEntity, RoleModel>().IgnoreAllNonExisting()
                .ForMember(d => d.Permissions, o => o.MapFrom(s => s.Permissions.Select(x => x.Permission)));
        }
    }
}