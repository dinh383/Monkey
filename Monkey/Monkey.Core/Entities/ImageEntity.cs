#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Entity Map </Project>
//     <File>
//         <Name> ImageEntity.cs </Name>
//         <Created> 10/10/17 8:01:05 PM </Created>
//         <Key> 6d3ba270-a744-489d-908c-896eda8358ec </Key>
//     </File>
//     <Summary>
//         ImageEntity.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

namespace Monkey.Core.Entities
{
    public class ImageEntity : Entity
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string Extension { get; set; }

        public string MimeType { get; set; }

        public double ContentLength { get; set; }

        public string ImageDominantHexColor { get; set; }

        public int ImageWidthPx { get; set; }

        public int ImageHeightPx { get; set; }
        public string Caption { get; set; }
    }
}