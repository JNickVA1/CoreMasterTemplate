using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CityMaster
{
	public class Program
	{
		#region Fields
		#endregion Fields

		#region Constructors
		#endregion Constructors

		#region Properties
		#endregion Properties

		#region Methods
		/// <summary>
		/// The app entry point.
		/// </summary>
		/// <param name="args"></param>
		public static void Main(string[] args)
		{
			// Build and run the Web Application.
			CreateWebHostBuilder(args).Build().Run();
		}

		/// <summary>
		/// Build the IWebHost.
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
		#endregion Methods

		#region Event Handlers
		#endregion Event Handlers
	}
}
