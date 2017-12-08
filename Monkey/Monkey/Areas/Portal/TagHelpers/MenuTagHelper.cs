using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Monkey.Core;
using Monkey.Core.Constants;
using Puppy.Core.DictionaryUtils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monkey.Areas.Portal.TagHelpers
{
    [HtmlTargetElement(MenuTagName, TagStructure = TagStructure.NormalOrSelfClosing)]
    public class MenuTagHelper : TagHelper
    {
        public const string MenuTagName = "menu";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;

            output.TagName = "div";
            output.Attributes.Add("class", "site-menubar");

            var ul = new TagBuilder("ul");
            ul.AddCssClass("site-menu");

            var childContent = await output.GetChildContentAsync().ConfigureAwait(true);

            ul.InnerHtml.SetHtmlContent(childContent);

            output.Content.SetHtmlContent(ul);
        }
    }

    [HtmlTargetElement(MenuItemTagName, TagStructure = TagStructure.NormalOrSelfClosing)]
    public class MenuItemTagHelper : TagHelper
    {
        public const string MenuItemTagName = "menu-item";

        [HtmlAttributeName("menu-name")]
        public string MenuName { get; set; }

        [HtmlAttributeName("menu-url")]
        public string MenuUrl { get; set; }

        [HtmlAttributeName("menu-icon-class")]
        public string MenuIconClass { get; set; }

        [HtmlAttributeName("menu-has-sub")]
        public bool IsHasSubItem { get; set; }

        [HtmlAttributeName("menu-permissions")]
        public IEnumerable<Enums.Permission> Permissions { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Permissions?.Any() == true && !LoggedInUser.Current.IsHaveAnyPermissions(Permissions))
            {
                return;
            }

            output.TagMode = TagMode.StartTagAndEndTag;

            bool isActive = !string.IsNullOrWhiteSpace(MenuUrl) && System.Web.HttpContext.Current?.Request.GetDisplayUrl().Contains(MenuUrl) == true;

            output.TagName = "li";
            output.Attributes.Add("class", $"site-menu-item {(IsHasSubItem ? "has-sub" : string.Empty)} {(isActive ? "active" : string.Empty)}");

            var anchor = new TagBuilder("a");
            anchor.Attributes.AddOrUpdate("href", $"{(IsHasSubItem ? "javascript:void(0);" : MenuUrl)}");

            var icon = new TagBuilder("i");
            icon.AddCssClass(MenuIconClass);
            icon.Attributes.AddOrUpdate("aria-hidden", "true");

            anchor.InnerHtml.AppendHtml(icon);

            var spanTitle = new TagBuilder("span");
            spanTitle.AddCssClass("site-menu-title");
            spanTitle.InnerHtml.Append(MenuName);

            anchor.InnerHtml.AppendHtml(spanTitle);

            if (IsHasSubItem)
            {
                var spanArrow = new TagBuilder("span");
                spanArrow.AddCssClass("site-menu-arrow");
                anchor.InnerHtml.AppendHtml(spanArrow);
            }

            output.Content.SetHtmlContent(anchor);

            if (IsHasSubItem)
            {
                var ul = new TagBuilder("ul");
                ul.AddCssClass("site-menu-sub");

                var childContent = await output.GetChildContentAsync().ConfigureAwait(true);

                ul.InnerHtml.SetHtmlContent(childContent);

                output.Content.AppendHtml(ul);
            }
        }
    }

    [HtmlTargetElement(SubMenuItemTagName, ParentTag = MenuItemTagHelper.MenuItemTagName, TagStructure = TagStructure.WithoutEndTag)]
    public class SubMenuItemTagHelper : TagHelper
    {
        public const string SubMenuItemTagName = "sub-menu-item";

        [HtmlAttributeName("sub-menu-name")]
        public string SubMenuName { get; set; }

        [HtmlAttributeName("sub-menu-url")]
        public string SubMenuUrl { get; set; }

        [HtmlAttributeName("sub-menu-permissions")]
        public IEnumerable<Enums.Permission> Permissions { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Permissions?.Any() == true && !LoggedInUser.Current.IsHaveAnyPermissions(Permissions))
            {
                return;
            }

            output.TagMode = TagMode.StartTagAndEndTag;

            output.TagName = "li";

            bool isActive = !string.IsNullOrWhiteSpace(SubMenuUrl) && System.Web.HttpContext.Current?.Request.GetDisplayUrl().Contains(SubMenuUrl) == true;

            output.Attributes.Add("class", $"site-menu-item {(isActive ? "active" : string.Empty)}");

            var anchor = new TagBuilder("a");
            anchor.AddCssClass("animsition-link");
            anchor.Attributes.AddOrUpdate("href", SubMenuUrl);

            var spanTitle = new TagBuilder("span");
            spanTitle.AddCssClass("site-menu-title");
            spanTitle.InnerHtml.Append(SubMenuName);
            anchor.InnerHtml.AppendHtml(spanTitle);

            output.Content.SetHtmlContent(anchor);
        }
    }
}