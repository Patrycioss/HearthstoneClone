using UnityEngine;

/// <summary>
/// Used for interaction with a button.
/// </summary>
public abstract class GameAction : MonoBehaviour
{
	/// <summary>
	/// Called when the button is clicked.
	/// </summary>
	public abstract void Act();
}