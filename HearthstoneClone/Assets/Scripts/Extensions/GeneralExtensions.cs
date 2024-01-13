using System.Threading;

namespace Extensions
{
	/// <summary>
	/// General extension methods.
	/// </summary>
	public static class GeneralExtensions
	{
		/// <summary>
		/// Disposes the source and then makes a new one.
		/// </summary>
		/// <param name="tokenSource">Source of a cancellation token.</param>
		/// <returns>A new source.</returns>
		public static CancellationTokenSource Reset(this CancellationTokenSource tokenSource)
		{
			tokenSource?.Dispose();
			return new CancellationTokenSource();
		}
	}
}