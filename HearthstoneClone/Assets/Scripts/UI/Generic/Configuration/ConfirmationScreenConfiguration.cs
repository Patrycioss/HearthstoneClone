using System;
using JetBrains.Annotations;

namespace UI.Generic
{
	/// <summary>
	/// Configuration used to setup <see cref="ConfirmationScreen"/>.
	/// </summary>
	[Serializable]
	public class ConfirmationScreenConfiguration
	{
		/// <summary>
		/// General text to ask whether the user is sure about what they want to do.
		/// </summary>
		[NotNull] public string MessageText = "Are you sure you wish to continue?";
			
		/// <summary>
		/// Text on the continue button.
		/// </summary>
		[NotNull] public string ConfirmButtonText = "Continue";
			
		/// <summary>
		/// Text on the cancel button.
		/// </summary>
		[NotNull] public string CancelButtonText = "Cancel";
	}
}