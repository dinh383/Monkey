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
using Microsoft.AspNetCore.Http;
using Puppy.DataTable.Attributes;

namespace Monkey.Core.Models
{
    public class ImageModel
    {
        [DataTable(IsVisible = false, Order = 1)]
        public int Id { get; set; }

        [DataTableIgnore]
        public string GlobalId { get; set; }

        [DataTableIgnore]
        public DateTimeOffset CreatedTime { get; set; }

        [DataTableIgnore]
        public string Name { get; set; }

        [DataTableIgnore]
        public string Url { get; set; }

        public string Extension { get; set; }

        [DataTableIgnore]
        public string MineType { get; set; }

        [DataTableIgnore]
        public double ContentLength { get; set; }

        public string ImageDominantHexColor { get; set; }

        [DataTableIgnore]
        public int ImageWidthPx { get; set; }

        [DataTableIgnore]
        public int ImageHeightPx { get; set; }

        public string Caption { get; set; }
    }

    public class ImageAddModel : ImageModel
    {
        public IFormFile File { get; set; }
    }
}