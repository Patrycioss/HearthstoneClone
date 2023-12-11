using JetBrains.Annotations;

namespace UI.Generic
{
	/// <summary>
	/// Configuration class for a generic message.
	/// </summary>
	[NotNull]
	public class MessageConfiguration
	{
		/// <summary>
		/// Text on the message.
		/// </summary>
		[NotNull]
		public string MessageText = string.Empty;
			
		/// <summary>
		/// Text on the button continue button.
		/// </summary>
		[NotNull]
		public string ButtonText = "Continue";
	}
}