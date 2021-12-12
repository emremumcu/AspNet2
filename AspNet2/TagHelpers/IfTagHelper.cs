using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KPSNufusOlayEs.TagHelpers
{
    //https://andrewlock.net/creating-an-if-tag-helper-to-conditionally-render-content/ 

    [HtmlTargetElement(Attributes = "if")]
    public class IfTagHelper : TagHelper
    {
        public override int Order => -1000;

        [HtmlAttributeName("if")]
        public bool Include { get; set; } = true;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {            
            output.TagName = null; // Always strip the outer tag name as we never want <if> to render

            if (Include) return;
            else output.SuppressOutput();
        }
    }
}
