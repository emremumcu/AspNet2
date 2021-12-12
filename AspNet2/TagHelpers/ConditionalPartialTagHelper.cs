using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KPSNufusOlayEs.TagHelpers
{
    // https://github.com/reZach/minifynetcore/blob/master/MinifyNETCore/MinifyNETCore/TagHelpers/MPartialTagHelper.cs

    [HtmlTargetElement("partial-c", TagStructure = TagStructure.WithoutEndTag)]
    public class ConditionalPartialTagHelper : PartialTagHelper
    {
        [HtmlAttributeName("if")]
        public bool Include { get; set; } = true;

        public ConditionalPartialTagHelper(ICompositeViewEngine viewEngine, IViewBufferScope viewBufferScope) : base(viewEngine, viewBufferScope) { }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if(!Include) 
                return Task.CompletedTask;
            else 
                return base.ProcessAsync(context, output);
        }
    }
}
