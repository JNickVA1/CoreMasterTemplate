using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace CityMaster.Pages
{
	public class IndexModel : PageModelBase
	{
		#region Fields
		private readonly IStringLocalizer<IndexModel> _localizer;
		#endregion Fields

		#region Constructors
		public IndexModel(IStringLocalizer<IndexModel> localizer)
		{
			//
			_localizer = localizer;
		}
		#endregion Constructors

		#region Properties
		#endregion Properties

		#region Methods
		#endregion Methods

		#region Event Handlers
		public void OnGet()
		{

		}
		#endregion Event Handlers
	}
}
