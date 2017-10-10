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

using Monkey.Core.Entities;
using Puppy.DependencyInjection.Attributes;

namespace Monkey.Data.EF.Repositories
{
    [PerRequestDependency(ServiceType = typeof(IImageRepository))]
    public class ImageRepository : EntityRepository<ImageEntity>, IImageRepository
    {
        public ImageRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}