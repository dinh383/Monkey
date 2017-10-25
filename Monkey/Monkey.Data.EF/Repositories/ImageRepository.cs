#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Repository </Project>
//     <File>
//         <Name> ImageRepository.cs </Name>
//         <Created> 10/10/17 8:09:33 PM </Created>
//         <Key> 9615f1d1-76b6-4164-bfe4-ad7b682e6d0c </Key>
//     </File>
//     <Summary>
//         ImageRepository.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core;
using Monkey.Core.Constants;
using Monkey.Core.Entities;
using Monkey.Core.Models;
using Microsoft.AspNetCore.Http;
using Puppy.AutoMapper;
using Puppy.Core.FileUtils;
using Puppy.Core.StringUtils;
using Puppy.DependencyInjection.Attributes;
using System;
using System.IO;
using System.Linq;

namespace Monkey.Data.EF.Repositories
{
    [PerRequestDependency(ServiceType = typeof(IImageRepository))]
    public class ImageRepository : EntityRepository<ImageEntity>, IImageRepository
    {
        public ImageRepository(IDbContext dbContext) : base(dbContext)
        {
        }

        public ImageModel SaveImage(IFormFile file, string caption = "", string imageDominantHexColor = null)
        {
            if (file == null || file.Length <= 0)
            {
                return null;
            }

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);

                var stringBase64 = Convert.ToBase64String(stream.ToArray());

                var imageEntity = new ImageEntity();

                // Url and Save Path
                var fileName = $"{Path.GetFileName(file.FileName)}-{imageEntity.GlobalId}{Path.GetExtension(file.FileName)}";
                fileName = fileName.Replace(" ", "-").ToLowerInvariant();
                fileName = FileHelper.MakeValidFileName(fileName);

                imageEntity.Url = Path.Combine(PathConsts.UploadFolder, fileName);
                var savePath = SystemUtils.GetWebPhysicalPath(imageEntity.Url);

                // Save to Physical
                var fileModel = FileHelper.Save(stringBase64, file.FileName, savePath);

                // Update Image Entity by FileModel
                fileModel.MapTo(imageEntity);
                imageEntity.Url = SystemUtils.GetWebUrl(fileModel.Location);

                imageEntity.Caption = caption;
                if (!String.IsNullOrWhiteSpace(imageDominantHexColor))
                {
                    imageEntity.ImageDominantHexColor = imageDominantHexColor;
                }

                Add(imageEntity);
                SaveChanges();

                var imageModel = imageEntity.MapTo<ImageModel>();
                return imageModel;
            }
        }

        public void RemoveImage(int id)
        {
            var path = Get(x => x.Id == id).Select(x => x.Url).Single();

            path = path.GetFullPath();

            // Delete physical
            FileHelper.SafeDelete(path);

            // Delete database
            Delete(new ImageEntity { Id = id }, true);

            SaveChanges();
        }
    }
}