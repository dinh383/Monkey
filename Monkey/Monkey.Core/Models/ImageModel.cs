#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> ImageModel.cs </Name>
//         <Created> 10/10/17 11:33:21 PM </Created>
//         <Key> a2d11f2c-a87b-4b84-8378-1010f365f3d1 </Key>
//     </File>
//     <Summary>
//         ImageModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using System;

namespace Monkey.Core.Models
{
    public class ImageModel
    {
        public int Id { get; set; }

        public string GlobalId { get; set; }

        public DateTimeOffset CreatedTime { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Extension { get; set; }

        public string MineType { get; set; }

        public double ContentLength { get; set; }

        public string ImageDominantHexColor { get; set; }

        public int ImageWidthPx { get; set; }

        public int ImageHeightPx { get; set; }
    }
}