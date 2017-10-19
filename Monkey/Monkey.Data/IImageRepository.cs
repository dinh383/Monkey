#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Repository Interface </Project>
//     <File>
//         <Name> IImageRepository.cs </Name>
//         <Created> 10/10/17 8:09:03 PM </Created>
//         <Key> 6487853b-a367-4957-bdd7-46c2f2ba4418 </Key>
//     </File>
//     <Summary>
//         IImageRepository.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Entities;
using Monkey.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Monkey.Data
{
    public interface IImageRepository : IEntityRepository<ImageEntity>
    {
        /// <summary>
        ///     Save image file to physical and database as well 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        ImageModel SaveImage(IFormFile file);

        /// <summary>
        ///     Remove image file from physical and database as well 
        /// </summary>
        /// <param name="id"></param>
        void RemoveImage(int id);
    }
}