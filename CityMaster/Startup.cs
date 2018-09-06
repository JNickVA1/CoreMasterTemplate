using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityMaster.Data.DbContexts;
using CityMaster.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CityMaster
{
	public class Startup
	{
		#region Fields
		// Define the constant for the default culture, US English, using the IETF RFC 4646 guidelines.
		// See: https://en.wikipedia.org/wiki/IETF_language_tag
		private readonly string DefaultCulture;
		#endregion Fields

		#region Constructors
		/// <summary>
		/// The Startup class, or any class used in the WebHost UseStartup method call, can be constructed
		/// in various ways depending on the objects required as constructor parameters. For example, all
		/// of the following are valid constructors for the startup class:
		/// 1. public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
		/// 2. public Startup(IHostingEnvironment env)
		/// 3. public Startup(IConfiguration configuration)
		/// There may be more variations but I have yet to find definitive documentation. The code that is
		/// implemented in ASP.NET Core can be see at: https://github.com/aspnet/Hosting, which is a good
		/// place to start to try to figure out what is, and isn't, allowed as startup class parameters.
		/// </summary>
		/// <param name="env"></param>
		public Startup(IHostingEnvironment env)
		{
			Environment = env;

			// Set up configuration sources.
			SetupConfiguration();
		}
		#endregion Constructors

		#region Properties
		/// <summary>
		/// The set of key/value configuration properties.
		/// </summary>
		public static IConfiguration Configuration { get; set; }

		/// <summary>
		/// A variable used to hold runtime environment information.
		/// </summary>
		public static IHostingEnvironment Environment { get; set; }

		/// <summary>
		/// Used to configure logging and to create ILogger instances.
		/// </summary>
		public ILoggerFactory LoggerFactory { get; private set; }
		#endregion Properties

		#region Methods
		/// <summary>
		/// Initialize the app configuration using, primarily, the appsettings.json file
		/// as the source for the key/value pairs.
		/// </summary>
		private static void SetupConfiguration()
		{
			// Set up configuration sources.
			var builder = new ConfigurationBuilder()
				.SetBasePath(Environment.ContentRootPath)
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{Environment.EnvironmentName}.json", optional: true);

			// For the Development environment, add the User Secrets configuration source to the
			// IConfigurationBuilder. Perform all other Development configuration here as well.
			if (Environment.IsDevelopment())
			{
				// For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
				builder.AddUserSecrets<Startup>();
			}
			// Add environment variables and their values to the IConfigurationBuilder.
			// NOTE: Usually this is not necessary or desired in a development environment.
			//		 However, if running the app in an Azure App Service, then environment 
			//		 variables can be easily added that are only available in Azure.
			//		 Alternatively, and a much more secure approach, is to use the Azure Key Vault
			//		 when running the app in an Azure App Service. See the following for ,more information:
			//		 https://docs.microsoft.com/en-us/aspnet/core/security/key-vault-configuration?tabs=aspnetcore2x&view=aspnetcore-2.1
			builder.AddEnvironmentVariables();

			// Build the configuration from the set of sources previously added to the builder.
			Configuration = builder.Build();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			//
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});
			// Set the level of compatibility with ASP.NET Core.
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1); 

			// Configure the MVC framework to require HTTPS.
			services.Configure<MvcOptions>(options =>
			{
				options.Filters.Add(new RequireHttpsAttribute());
			});
			// Add the database context using the connection string appropriate to the environment.
			if (Environment.IsDevelopment())
			{
				services.AddDbContext<ApplicationDbContext>(options =>
					options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
			}
			else if(Environment.IsEnvironment("Test"))
			{
				services.AddDbContext<ApplicationDbContext>(options =>
					options.UseSqlServer(Configuration.GetConnectionString("AzureTestConnection")));
			}
			else if (Environment.IsProduction())
			{
				services.AddDbContext<ApplicationDbContext>(options =>
					options.UseSqlServer(Configuration.GetConnectionString("AzureProdConnection")));
			}
			else
			{
				//
			}
			//
			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();
			
			// Add the localization services to the services container
			services.AddLocalization(options => options.ResourcesPath = "Resources");

			//
			services.AddMvc()
				// Add support for finding localized views, based on file name suffix, e.g. Index.fr.cshtml
				.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
				// Add support for localizing strings in data annotations (e.g. validation messages) via the
				// IStringLocalizer abstractions.
				.AddDataAnnotationsLocalization();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();

			app.UseMvc();
		}
		#endregion Methods

		#region Event Handlers
		#endregion Event Handlers
	}
}
