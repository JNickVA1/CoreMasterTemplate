#pragma checksum "C:\Users\JNickVA1\Source\Repos\CoreMasterTemplate\CityMaster\Pages\About.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a54c3c6e83893eac2de5777fd3b5f8a5645a52a5"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(CityMaster.Pages.Pages_About), @"mvc.1.0.razor-page", @"/Pages/About.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Pages/About.cshtml", typeof(CityMaster.Pages.Pages_About), null)]
namespace CityMaster.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\JNickVA1\Source\Repos\CoreMasterTemplate\CityMaster\Pages\_ViewImports.cshtml"
using CityMaster;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a54c3c6e83893eac2de5777fd3b5f8a5645a52a5", @"/Pages/About.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"14b9d1af35bfc3c9260865221716187690601313", @"/Pages/_ViewImports.cshtml")]
    public class Pages_About : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "C:\Users\JNickVA1\Source\Repos\CoreMasterTemplate\CityMaster\Pages\About.cshtml"
  
	ViewData["Title"] = "About";

#line default
#line hidden
            BeginContext(64, 4, true);
            WriteLiteral("<h2>");
            EndContext();
            BeginContext(69, 17, false);
#line 6 "C:\Users\JNickVA1\Source\Repos\CoreMasterTemplate\CityMaster\Pages\About.cshtml"
Write(ViewData["Title"]);

#line default
#line hidden
            EndContext();
            BeginContext(86, 11, true);
            WriteLiteral("</h2>\r\n<h3>");
            EndContext();
            BeginContext(98, 13, false);
#line 7 "C:\Users\JNickVA1\Source\Repos\CoreMasterTemplate\CityMaster\Pages\About.cshtml"
Write(Model.Message);

#line default
#line hidden
            EndContext();
            BeginContext(111, 9, true);
            WriteLiteral("</h3>\r\n\r\n");
            EndContext();
            DefineSection("PageImageSection", async() => {
                BeginContext(146, 76, true);
                WriteLiteral("\r\n\t<div class=\"container-fluid\">\r\n\t\t<div id=\"myCarousel\" class=\"carousel\">\r\n");
                EndContext();
                BeginContext(498, 7, true);
                WriteLiteral("\t\t\t<img");
                EndContext();
                BeginWriteAttribute("src", " src=\"", 505, "\"", 536, 1);
#line 15 "C:\Users\JNickVA1\Source\Repos\CoreMasterTemplate\CityMaster\Pages\About.cshtml"
WriteAttributeValue("", 511, Url.Content(Model.Image), 511, 25, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(537, 97, true);
                WriteLiteral(" alt=\"ASP.NET\" class=\"img-responsive\"/>\r\n\t\t\t<div class=\"carousel-caption\" role=\"option\">\r\n\t\t\t\t<p>");
                EndContext();
                BeginContext(635, 18, false);
#line 17 "C:\Users\JNickVA1\Source\Repos\CoreMasterTemplate\CityMaster\Pages\About.cshtml"
              Write(Model.ImageCaption);

#line default
#line hidden
                EndContext();
                BeginContext(653, 36, true);
                WriteLiteral("</p>\r\n\t\t\t</div>\r\n\t\t</div>\r\n\t</div>\r\n");
                EndContext();
            }
            );
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<AboutModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<AboutModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<AboutModel>)PageContext?.ViewData;
        public AboutModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
