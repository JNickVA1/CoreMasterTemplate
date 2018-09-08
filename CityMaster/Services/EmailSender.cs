using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CityMaster.Services
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// This class is used by the application to send email for account confirmation and password reset.
	/// For more details see https://go.microsoft.com/fwlink/?LinkID=532713
	/// </remarks>
	public class EmailSender : IEmailSender
    {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="optionsAccessor"></param>
		public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
		{
			Options = optionsAccessor.Value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="email"></param>
		/// <param name="subject"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public Task SendEmailAsync(string email, string subject, string message)
        {
			dynamic result = Execute(Options.SendGridKey, subject, message, email);
			//return Execute(Options.SendGridKey, subject, message, email);
			return result;
	        //return Task.CompletedTask;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="apiKey"></param>
		/// <param name="subject"></param>
		/// <param name="message"></param>
		/// <param name="email"></param>
		/// <returns></returns>
		public Task Execute(string apiKey, string subject, string message, string email)
		{
			var client = new SendGridClient(apiKey);
			var msg = new SendGridMessage()
			{
				From = new EmailAddress("NicholsonSoftware@hotmail.com", "Jim Nicholson"),
				Subject = subject,
				PlainTextContent = message,
				HtmlContent = message
			};
			msg.AddTo(new EmailAddress(email));
			return client.SendEmailAsync(msg);
		}

		/// <summary>
		/// 
		/// </summary>
		public AuthMessageSenderOptions Options { get; } //set only via Secret Manager
	}
}
