using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Extensions
{
	/// <summary>
	/// Tween extension methods.
	/// </summary>
	public static class TweenExtensions
	{
		/// <summary>
		/// Makes a fast-forwardable task that can be fast-forwarded using a <see cref="CancellationToken"/>. 
		/// </summary>
		/// <param name="tween">The <see cref="Tween"/> to make fast-forwardable.</param>
		/// <param name="cancellationToken">The token to use.</param>
		/// <returns>An awaitable task.</returns>
		public static Task AsFastForwardTask(this Tween tween, CancellationToken cancellationToken)
		{
			var taskCompletionSource = new TaskCompletionSource<bool>();

			tween.OnComplete(() => { taskCompletionSource.TrySetResult(true); });
			tween.OnKill(() => { taskCompletionSource.TrySetCanceled(); });

			if (cancellationToken != CancellationToken.None)
			{
				tween.OnUpdate(() =>
				{
					if (cancellationToken.IsCancellationRequested)
					{
						tween.Complete();
						tween.Kill();
					}
				});
			}
			else
			{
				Debug.LogWarning($"No valid token passed on for this tween!");
			}
			
			return taskCompletionSource.Task;
		}
	}
}