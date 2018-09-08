namespace CityMaster.Services
{
	/// <summary>
	/// The object used to access the user account and key settings for SendGrid.
	/// </summary>
	public class AuthMessageSenderOptions
    {
		/// <summary>
		/// The SendGrid account user ID.
		/// </summary>
		public string SendGridUser { get; set; }
		/// <summary>
		/// The SendGrid API key.
		/// </summary>
		public string SendGridKey { get; set; }
	}
}
