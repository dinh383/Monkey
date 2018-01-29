#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Repository </Project>
//     <File>
//         <Name> Entity.cs </Name>
//         <Created> 15/08/17 12:40:40 PM </Created>
//         <Key> 62331d34-8570-4240-9ce8-47c093e0f664 </Key>
//     </File>
//     <Summary>
//         Entity.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

namespace Monkey.Core.Entities
{
    public class Entity : Puppy.EF.Entities.Entity
    {
        public Entity()
        {
            CreatedTime = SystemUtils.SystemTimeNow;

            LastUpdatedTime = SystemUtils.SystemTimeNow;
        }
    }
}