#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> EntityMap.cs </Name>
//         <Created> 06/09/17 2:42:28 PM </Created>
//         <Key> 300c7101-92ce-40a6-aea5-043d3afa9d1d </Key>
//     </File>
//     <Summary>
//         EntityMap.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

namespace Monkey.Data.EF.Map.Base
{
    public abstract class EntityMap<T> : Puppy.EF.Mapping.EntityTypeConfiguration<T> where T : Entities.Entity
    {
    }
}