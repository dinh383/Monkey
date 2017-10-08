using AutoMapper;
using Monkey.Core.Entities.User;
using Monkey.Core.Models.Auth;
using Puppy.AutoMapper;

namespace Monkey.Mapper.Auth
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleEntity, RoleModel>().IgnoreAllNonExisting();
        }
    }
}