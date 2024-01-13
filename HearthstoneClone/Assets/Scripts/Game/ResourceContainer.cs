using System;

namespace Game
{
	/// <summary>
	/// Component that handles the amount of resources of something.
	/// </summary>
	public class ResourceContainer
	{
		/// <summary>
		/// Called whenever the amount of the resource changes.
		/// </summary>
		public event Action<int> OnChanged;

		/// <summary>
		/// Called whenever the amount reaches 0.
		/// </summary>
		public event Action OnEmpty;
		
		/// <summary>
		/// Total amount of resources.
		/// </summary>
		public int Total { get; private set; }

		/// <summary>
		/// Maximum amount of resources.
		/// <remarks>If set to 0 there is no limit.</remarks>
		/// </summary>
		public int Max { get; private set; }
		
		/// <summary>
		/// Determines whether the container enforces a maximum.
		/// Can be overruled with a boolean in the respective functions.
		/// </summary>
		public bool UsesMaximum { get; private set; }

		/// <summary>
		/// Makes a new resource container.
		/// </summary>
		/// <remarks>Enables <see cref="UsesMaximum"/>.</remarks>
		/// <param name="startAmount">The start amount of resources.</param>
		/// <param name="maxAmount">The maximum amount of resources.</param>
		public ResourceContainer(int startAmount = 0, int maxAmount = 0)
		{
			Total = startAmount;
			Max = maxAmount;
			UsesMaximum = true;
		}
		
		/// <summary>
		/// Makes a new resource container.
		/// <remarks>Disables <see cref="UsesMaximum"/>.</remarks>
		/// </summary>
		/// <param name="startAmount">Start amount of resources.</param>
		public ResourceContainer(int startAmount = 0)
		{
			Total = startAmount;
			UsesMaximum = false;
		}

		/// <summary>
		/// Sets the maximum and enables <see cref="UsesMaximum"/>.
		/// </summary>
		/// <param name="maximum">The new maximum.</param>
		public void SetMaximum(int maximum)
		{
			UsesMaximum = true;
			Max = maximum;
		}

		/// <summary>
		/// Disables <see cref="UsesMaximum"/>.
		/// </summary>
		public void SetMaximum()
		{
			UsesMaximum = false;
		}

		/// <summary>
		/// Add or remove an amount of resources.
		/// </summary>
		/// <remarks>This function functions both as adding and removing resources. When <see cref="Total"/>
		/// reaches equal or lesser than 0 <see cref="OnEmpty"/> is called.
		/// Otherwise if the <see cref="Total"/> has changed <see cref="OnChanged"/> is called.</remarks>
		/// <param name="amount">Amount to add.</param>
		/// <param name="ignoreMax">If set to true ignores the max amount of resources.</param>
		public void AddOrRemove(int amount, bool ignoreMax = false)
		{
			int currentTotal = Total;
			Total += amount;
			
			if (UsesMaximum && !ignoreMax)
			{
				if (Total > Max)
				{
					Total = Max;
				}
			}

			if (Total <= 0)
			{
				Total = 0;
				OnEmpty?.Invoke();
			}
			else if (Total != currentTotal)
			{
				OnChanged?.Invoke(Total);
			}
		}

		/// <summary>
		/// Checks if there's enough of the resource to remove, if not doesn't remove it.
		/// </summary>
		/// <param name="amount">Amount of the resource to remove.</param>
		/// <returns>Whether it can remove this amount.</returns>
		public bool TryRemove(int amount)
		{
			if (amount <= Total)
			{
				AddOrRemove(-amount);
				return true;
			}

			return false;
		}
	}
}