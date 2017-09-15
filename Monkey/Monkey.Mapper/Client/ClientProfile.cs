#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Mapper <Project>
//     <File>
//         <Name> ClientProfile.cs </Name>
//         <Created> 14/09/17 11:11:40 PM </Created>
//         <Key> 7d288776-4836-44d0-b6f6-526500bc67f4 </Key>
//     </File>
//     <Summary>
//         ClientProfile.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using AutoMapper;
using Monkey.Core.Entities.Client;
using Monkey.Core.Models.Client;
using Puppy.AutoMapper;
using Puppy.Core.StringUtils;

namespace Monkey.Mapper.Client
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<ClientCreateModel, ClientEntity>().IgnoreAllNonExisting()
                .ForMember(d => d.NameNorm, o => o.MapFrom(s => StringHelper.Normalize(s.Name)));

            CreateMap<ClientEntity, ClientModel>().IgnoreAllNonExisting();

            CreateMap<ClientUpdateModel, ClientEntity>().IgnoreAllNonExisting()
                .ForMember(d => d.NameNorm, o => o.MapFrom(s => StringHelper.Normalize(s.Name)));
        }
    }
}