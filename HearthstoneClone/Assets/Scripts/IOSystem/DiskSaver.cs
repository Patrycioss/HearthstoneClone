using System;
using System.IO;
using System.Threading.Tasks;
using ErrorHandling;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace IOSystem
{
	/// <summary>
	/// Saves a <see cref="Savable"/> from memory to disk.
	/// </summary>
	public class DiskSaver
	{
		/// <summary>
		/// Save a <see cref="object"/> to disk.
		/// </summary>
		/// <param name="savable"><see cref="Savable"/> object to save.</param>
		/// <returns>Whether the saving succeeded or not.</returns>
		[NotNull]
		public static async Task<Result> Save(Savable savable)
		{
			Result result = new Result();

			Result<string> pathResult = savable.GetPath();
			if (!pathResult)
			{
				return result + pathResult.Message;
			}

			// Path can't be null as we already handled failure result.
			string path = pathResult.Value!;
			string dataToStore;

			// Convert value to json.
			try
			{
				dataToStore = JsonConvert.SerializeObject(savable, Formatting.Indented);
			}
			catch (Exception e)
			{
				return result + $"Failed to convert {savable} to JSON as exception: {e} occurred!";
			}

			if (string.IsNullOrEmpty(dataToStore))
			{
				return result + $"Failed to convert {savable} to JSON without any exceptions!";
			}

			// Overwrite file if exists otherwise create new one and write into that one.
			if (File.Exists(path))
			{
				try
				{
					await File.WriteAllTextAsync(path, dataToStore);
				}
				catch (Exception e)
				{
					return result +
					       $"Failed to overwrite file at {path} with contents: {dataToStore}. Exception {e} occurred!";
				}

				result += $"Overwrote file at {path} with contents: {dataToStore}.";
			}
			else
			{
				try
				{
					await using (FileStream stream = new FileStream(path, FileMode.Create))
					{
						await using (StreamWriter writer = new StreamWriter(stream))
						{
							await writer.WriteAsync(dataToStore);
						}
					}
				}
				catch (Exception e)
				{
					return result + $"Failed to save {savable} to {path}. Exception {e} occurred!";
				}

				result += $"Saved new file at {path} with contents: {dataToStore}.";
			}

			result.Success = true;
			return result;
		}
	}
}