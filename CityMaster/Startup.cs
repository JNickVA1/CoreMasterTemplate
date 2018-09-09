using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CityMaster.Data.DbContexts;
using CityMaster.Models;
using CityMaster.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
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
		private static CultureInfo DefaultCulture;

		//
		private static List<CultureInfo> SupportedCultures = new List<CultureInfo>();
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
		/// <param name="loggerFactory"></param>
		public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			Environment = env;
			LoggerFactory = loggerFactory;

			// Set up configuration sources.
			SetupConfiguration();

			// Setup localization configuration.
			SetupLocalization();
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
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

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

		/// <summary>
		/// 
		/// </summary>
		private static void SetupLocalization()
		{
			// Read the default culture.
			DefaultCulture = new CultureInfo(Configuration["Localization:DefaultCulture"]);

			// Read all supported cultures and add each to the List of SupportedCultures.
			var cultures = Configuration.GetSection("Localization:SupportedCultures").GetChildren();
			foreach (var culture in cultures)
			{
				SupportedCultures.Add(new CultureInfo(culture.Value));
			}
		}

		/// <summary>
		/// This method gets called by the runtime. Use this method to add services to the container.
		/// </summary>
		/// <param name="services"></param>
		/// <remarks>
		/// Is called by the runtime BEFORE Configure().
		/// </remarks>
		public void ConfigureServices(IServiceCollection services)
		{
			// Configure the cookie policies.
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
			// NOTE: I am restricting the database context to connect only to a SQL Server database.
			//		 Since I intend to use code derived from this template only on Azure, and only
			//		 with SQL Server, I don't feel that it's necessary to use dependency inversion.
			if (Environment.IsDevelopment())
			{
				// I'm using the default connection string for development on my local machine.
				services.AddDbContext<ApplicationDbContext>(options =>
					options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

				// NOTE: Logging will be performed via one of, or both of, the standard loggers that
				//		 are configured in the call to CreateDefaultBuilder (Console and Debug).
			}
			else if(Environment.IsEnvironment("Test"))
			{
				// This is for the Azure Test environment.
				services.AddDbContext<ApplicationDbContext>(options =>
					options.UseSqlServer(Configuration.GetConnectionString("AzureTestConnection")));

				// Add the Azure App Service logging provider.
				// TODO: Configure the AzureAppServicesDiagnosticsSettings that can be passed into 
				// TODO: the call to AddAzureWebAppDiagnostics for Test logging.
				LoggerFactory.AddAzureWebAppDiagnostics();
			}
			else if (Environment.IsProduction())
			{
				// This is for the Azure Production environment.
				services.AddDbContext<ApplicationDbContext>(options =>
					options.UseSqlServer(Configuration.GetConnectionString("AzureProdConnection")));

				// Add the Azure App Service logging provider.
				// TODO: Configure the AzureAppServicesDiagnosticsSettings that can be passed into 
				// TODO: the call to AddAzureWebAppDiagnostics for Production logging.
				LoggerFactory.AddAzureWebAppDiagnostics();
			}
			else if (Environment.IsStaging())
			{
				// This is for the Azure Staging environment.
				services.AddDbContext<ApplicationDbContext>(options =>
					options.UseSqlServer(Configuration.GetConnectionString("AzureStagingConnection")));

				// Add the Azure App Service logging provider.
				// TODO: Configure the AzureAppServicesDiagnosticsSettings that can be passed into 
				// TODO: the call to AddAzureWebAppDiagnostics for Staging logging.
				LoggerFactory.AddAzureWebAppDiagnostics();
			}
			else
			{
				// Obviously we have some sort of malfunction and should never get here. 
				// Log the error and exit gracefully, if at all possible.
				// TODO: Test to determine what I can do here.
			}
			// Add the default Identity configuration to the Services Collection.
			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();
			
			// Add the localization services to the services container
			services.AddLocalization(options => options.ResourcesPath = "Resources");

			// Add MVC Services to the Services Collection.
			services.AddMvc()
				// Add support for finding localized views, based on file name suffix, e.g. Index.fr.cshtml
				.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
				// Add support for localizing strings in data annotations (e.g. validation messages) via the
				// IStringLocalizer abstractions.
				.AddDataAnnotationsLocalization();

			// Configure supported cultures and localization options
			services.Configure<RequestLocalizationOptions>(options =>
			{
				// State what the default culture for your application is. This will be used if no specific culture
				// can be determined for a given request.
				options.DefaultRequestCulture = new RequestCulture(DefaultCulture.Name, DefaultCulture.Name);

				// You must explicitly state which cultures your application supports.
				// These are the cultures the app supports for formatting numbers, dates, etc.
				options.SupportedCultures = SupportedCultures;

				// These are the cultures the app supports for UI strings, i.e. we have localized resources for.
				options.SupportedUICultures = SupportedCultures;

				// You can change which providers are configured to determine the culture for requests, or even add a custom
				// provider with your own logic. The providers will be asked in order to provide a culture for each request,
				// and the first to provide a non-null result that is in the configured supported cultures list will be used.
				// By default, the following built-in providers are configured:
				// - QueryStringRequestCultureProvider, sets culture via "culture" and "ui-culture" query string values, useful for testing
				// - CookieRequestCultureProvider, sets culture via "ASPNET_CULTURE" cookie
				// - AcceptLanguageHeaderRequestCultureProvider, sets culture via the "Accept-Language" request header
				//options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
				//{
				//  // My custom request culture logic
				//  return new ProviderCultureResult("en");
				//}));
			});

			// Register the email service used for "contacts".
			services.AddSingleton<IEmailSender, EmailSender>();

			// Configure startup to use the SendGrid options.
			services.Configure<AuthMessageSenderOptions>(Configuration);

			// Add cross-origin resource sharing services to the specified IServiceCollection.
			//
			// The Policy specifed as an option will allow any method.
			services.AddCors(options => options.AddPolicy("CorsPolicy", b => b.AllowAnyMethod()));
		}

		/// <summary>
		/// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		/// </summary>
		/// <param name="app"></param>
		/// <param name="env"></param>
		/// <remarks>
		/// Is called by the runtime AFTER ConfigureServices().
		/// </remarks>
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
