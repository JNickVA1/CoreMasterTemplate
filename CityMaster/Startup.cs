using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CityMaster.Data.DbContexts;
using CityMaster.Models;
using CityMaster.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
		/// <param name="configuration"></param>
		/// <param name="environment"></param>
		public Startup(IConfiguration configuration, IHostingEnvironment environment, ILoggerFactory loggerFactory)
		{
			Configuration = configuration;
			Environment = environment;
			LoggerFactory = loggerFactory;

			// Setup localization configuration.
			InitCultures();
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
		//private static void SetupConfiguration()
		//{
		//	// Set up configuration sources.
		//	var builder = new ConfigurationBuilder()
		//		.SetBasePath(Environment.ContentRootPath)
		//		.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
		//		.AddJsonFile($"appsettings.{Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

		//	// For the Development environment, add the User Secrets configuration source to the
		//	// IConfigurationBuilder. Perform all other Development configuration here as well.
		//	if (Environment.IsDevelopment())
		//	{
		//		// For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
		//		builder.AddUserSecrets<Startup>();
		//	}
		//	// Add environment variables and their values to the IConfigurationBuilder.
		//	// NOTE: Usually this is not necessary or desired in a development environment.
		//	//		 However, if running the app in an Azure App Service, then environment 
		//	//		 variables can be easily added that are only available in Azure.
		//	//		 Alternatively, and a much more secure approach, is to use the Azure Key Vault
		//	//		 when running the app in an Azure App Service. See the following for ,more information:
		//	//		 https://docs.microsoft.com/en-us/aspnet/core/security/key-vault-configuration?tabs=aspnetcore2x&view=aspnetcore-2.1
		//	builder.AddEnvironmentVariables();

		//	// Build the configuration from the set of sources previously added to the builder.
		//	Configuration = builder.Build();
		//}

		/// <summary>
		/// 
		/// </summary>
		private static void InitCultures()
		{
			// Read the default culture.
			DefaultCulture = new CultureInfo(Configuration["Localization:DefaultCulture"]);

			// Read all supported cultures and add each to the List of SupportedCultures.
			var cultures = Configuration.GetSection("Localization:SupportedCultures").GetChildren();
			foreach (var culture in cultures)
			{
				//
				var cultureInfo = new CultureInfo(culture.Value);

				//
				if (cultureInfo.EnglishName.StartsWith("Arabic"))
				{
					//
					cultureInfo.DateTimeFormat = new DateTimeFormatInfo() { Calendar = new GregorianCalendar() };

					//
					cultureInfo.NumberFormat = new NumberFormatInfo() {NativeDigits = "0 1 2 3 4 5 6 7 8 9".Split(" ")};
				}

				//
				SupportedCultures.Add(cultureInfo);
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

			//
			services.Configure<RequestLocalizationOptions>(ops =>
			{
				// State what the default culture for your application is. This will be used if no specific culture
				// can be determined for a given request.
				ops.DefaultRequestCulture = new RequestCulture(DefaultCulture);
				// You must explicitly state which cultures your application supports.
				// These are the cultures the app supports for formatting numbers, dates, etc.
				ops.SupportedCultures = SupportedCultures.OrderBy(x => x.EnglishName).ToList();
				// These are the cultures the app supports for UI strings, i.e. we have localized resources for.
				ops.SupportedUICultures = SupportedCultures.OrderBy(x => x.EnglishName).ToList();
			});

			// Set the level of compatibility with ASP.NET Core.
			services.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			// Add Localization for Data Annotations and set the option for finding resource files.
			services.AddMvc()
				// Add support for finding localized views, based on file name suffix, e.g. Index.fr.cshtml
				.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
				// Add support for localizing strings in data annotations (e.g. validation messages) via the
				// IStringLocalizer abstractions.
				.AddDataAnnotationsLocalization();

			// Add the localization services to the services container and set the path for Resources.
			services.AddLocalization(options => options.ResourcesPath = "Resources");

			// Configure the MVC framework to require HTTPS.
			services.Configure<MvcOptions>(options =>
			{
				options.Filters.Add(new RequireHttpsAttribute());
			});
			#region DbContexts
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
			#endregion DbContexts
			// Add the default Identity configuration to the Services Collection.
			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			// Register the email service used for "contacts".
			services.AddSingleton<IEmailSender, EmailSender>();

			// Configure startup to use the SendGrid options.
			services.Configure<AuthMessageSenderOptions>(Configuration);

			// Add cross-origin resource sharing services to the specified IServiceCollection.
			//
			// The Policy specified as an option will allow any method.
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
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}

			// For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
			try
			{
				using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
					.CreateScope())
				{
					serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
						.Database.Migrate();
				}
			}
			catch { }

			//
			app.UseRequestLocalization(new RequestLocalizationOptions
			{
				DefaultRequestCulture = new RequestCulture(DefaultCulture.Name),
				// Formatting numbers, dates, etc.
				SupportedCultures = SupportedCultures,
				// UI strings that we have localized.
				SupportedUICultures = SupportedCultures
			});

			//
			app.UseStaticFiles();
			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseCookiePolicy();
			app.UseMvc();
		}
		#endregion Methods

		#region Event Handlers
		#endregion Event Handlers
	}
}
