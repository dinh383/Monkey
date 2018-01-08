using Microsoft.AspNetCore.Razor.TagHelpers;
using Monkey.Core;
using Monkey.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Monkey.Areas.Portal.TagHelpers
{
    public enum SecurityMode
    {
        Hide,
        Disable,
        Readonly
    }

    [HtmlTargetElement("input")]
    [HtmlTargetElement("a")]
    [HtmlTargetElement("textarea")]
    [HtmlTargetElement("select")]
    [HtmlTargetElement("button")]
    [HtmlTargetElement("div")]
    [HtmlTargetElement("span")]
    [HtmlTargetElement("label")]
    [HtmlTargetElement("li")]
    [HtmlTargetElement("ul")]
    [HtmlTargetElement("nav")]
    [HtmlTargetElement("script")]
    [HtmlTargetElement("style")]
    public class SecurityTagHelper : TagHelper
    {
        private const string SecurityModeAttributeName = "asp-security-mode";

        private const string PermissionsAttributeName = "asp-permissions";

        [HtmlAttributeName(SecurityModeAttributeName)]
        public SecurityMode Mode { set; get; }

        [HtmlAttributeName(PermissionsAttributeName)]
        public IEnumerable<Enums.Permission> Permissions { set; get; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Permissions?.Any() == true && LoggedInUser.Current?.IsHaveAnyPermissions(Permissions) != true)
            {
                switch (Mode)
                {
                    case SecurityMode.Hide:
                        {
                            output.SuppressOutput();

                            return;
                        }
                    case SecurityMode.Disable:
                        {
                            var attribute = new TagHelperAttribute("disabled", "disabled");

                            output.Attributes.Add(attribute);

                            break;
                        }
                    case SecurityMode.Readonly:
                        {
                            var attribute = new TagHelperAttribute("readonly", "readonly");

                            output.Attributes.Add(attribute);

                            break;
                        }
                    default:
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                }
            }

            base.Process(context, output);
        }
    }
}