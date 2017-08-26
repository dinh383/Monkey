#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> PagedCollectionParametersConfigModel.cs </Name>
//         <Created> 27/08/17 1:01:16 AM </Created>
//         <Key> 0533c8dc-66ee-4045-bb10-bf19a72a4b1c </Key>
//     </File>
//     <Summary>
//         PagedCollectionParametersConfigModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

namespace Monkey.Core.ConfigModels
{
    public class PagedCollectionParametersConfigModel
    {
        public int Skip { get; set; } = 0;

        public int Take { get; set; } = 10;

        public int MaxTake { get; set; } = 10000;

        public string Terms { get; set; } = string.Empty;
    }
}