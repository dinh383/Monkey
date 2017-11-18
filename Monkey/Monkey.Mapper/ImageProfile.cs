#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Mapper <Project>
//     <File>
//         <Name> ImageProfile.cs </Name>
//         <Created> 10/10/17 11:35:23 PM </Created>
//         <Key> 02722e8f-dd44-4f32-84b8-1f7891b5fdd1 </Key>
//     </File>
//     <Summary>
//         ImageProfile.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using AutoMapper;
using Monkey.Core.Entities;
using Monkey.Core.Models;
using Puppy.AutoMapper;
using Puppy.Core.FileUtils;

namespace Monkey.Mapper
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<ImageEntity, ImageModel>().IgnoreAllNonExisting();
            CreateMap<ImageModel, UpdateImageModel>().IgnoreAllNonExisting();
            CreateMap<UpdateImageModel, ImageEntity>().IgnoreAllNonExisting();
            CreateMap<FileModel, ImageEntity>()
                .IgnoreAllNonExisting()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.OriginalFileName));
        }
    }
}