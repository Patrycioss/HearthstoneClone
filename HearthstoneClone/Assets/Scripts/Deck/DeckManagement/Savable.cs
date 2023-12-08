using System;
using System.IO;
using ErrorHandling;
using UnityEngine;

namespace Deck.DeckManagement
{
	/// <summary>
	/// Contains information of what to save and where to save it.
	/// </summary>
	[Serializable]
	public abstract class Savable
	{
		/// <summary>
		/// Directory to save to.
		/// </summary>
		public readonly SaveDirectory Directory;

		/// <summary>
		/// IDENTIFIER
		/// </summary>
		public Guid Identifier;

		/// <summary>
		/// Get the path to the file that the savable should be saved in.
		/// </summary>
		/// <returns>A <see cref="Result{T}"/> where T is a string containing the path to the destination file.</returns>
		public Result<string> GetPath()
		{
			Result<string> result = new Result<string>();

			string identifierString = Identifier.ToString();
			
			if (string.IsNullOrEmpty(identifierString))
			{
				return result + $"Failed to make a path for {ToString()} as Name is empty!";
			}
			
			try
			{
				result.Value = Path.Combine(Application.persistentDataPath, Directory.ToString(), identifierString);
			}
			catch (Exception e)
			{
				return result + $"Failed to make a path for {ToString()} as exception {e} occurred!";
			}

			result.Success = true;
			return result;
		}

		public override string ToString()
		{
			return $"(Type: {GetType()}, Directory: {Directory}, Identifier: {Identifier})";
		}
		
		public Savable(SaveDirectory directory = SaveDirectory.None)
		{
			Identifier = Guid.NewGuid();
			Directory = directory;
		}
	}
}