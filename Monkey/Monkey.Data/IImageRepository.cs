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

namespace Monkey.Data
{
	public interface IImageRepository: IEntityRepository<ImageEntity> 
	{
	}
}