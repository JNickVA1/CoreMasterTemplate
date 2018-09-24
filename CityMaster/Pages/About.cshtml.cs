using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CityMaster.Pages
{
	public class AboutModel : PageModelBase
	{
		public string Message { get; set; }
		public string Image { get; set; } = "~/images/Static/1140x360_Kumasi1.jpg";
		public string ImageCaption { get; set; } = "Test caption";

		public void OnGet()
		{
			Message = "Your application description page.";
		}
	}
}
