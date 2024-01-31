﻿/// <summary>
/// Can be used to transfer an object between scenes.
/// </summary>
public struct Transferable
{
	/// <summary>
	/// Value of the transferable.
	/// </summary>
	public readonly object Value;

	public Transferable(object value)
	{
		Value = value;
	}
}