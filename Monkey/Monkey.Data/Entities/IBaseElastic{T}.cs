#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Interface </Project>
//     <File>
//         <Name> IBaseElastic.cs </Name>
//         <Created> 10 May 17 10:20:53 AM </Created>
//         <Key> 419beb99-2307-4b2b-be05-482fc0c7e0b4 </Key>
//     </File>
//     <Summary>
//         IBaseElastic.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

namespace Monkey.Data.Entities
{
    public interface IBaseElastic
    {
    }

    public interface IBaseElastic<T> : IBaseElastic
    {
        T Id { get; set; }
    }
}