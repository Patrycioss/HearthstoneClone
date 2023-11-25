using JetBrains.Annotations;

namespace ErrorHandling
{
	/// <inheritdoc cref="Result"/>
	/// <typeparam name="T">Type of the value to be returned.</typeparam>
	public class Result<T> : Result
	{
		/// <summary>
		/// The value of the object.
		/// </summary>
		[CanBeNull] public T Value;
		
		public static Result<T> operator +(Result<T> result, string addition)
		{
			result.Message += addition;
			return result;
		}
		
		public static Result<T> operator +(Result<T> result, Message addition)
		{
			result.Message += addition;
			return result;
		}
		
		public static bool operator !(Result<T> result)
		{
			return !result.Success;
		}

		public static bool operator true(Result<T> result)
		{
			return result.Success;
		}

		public static bool operator false(Result<T> result)
		{
			return !result.Success;
		}
	}

	/// <summary>
	/// Object that contains whether an operation was successful and if not a <see cref="Message"/> containing what went wrong.
	/// <remarks>Pay attention that success defaults to false.</remarks>
	/// </summary>
	public class Result
	{
		/// <summary>
		/// Was the operation a success.
		/// </summary>
		public bool Success = false;

		/// <summary>
		/// Message that can indicate an error or something else.
		/// </summary>
		public Message Message = new Message();
		
		public static Result operator +(Result result, string addition)
		{
			result.Message += addition;
			return result;
		}
		
		public static Result operator +(Result result, Message addition)
		{
			result.Message += addition;
			return result;
		}
		
		public static bool operator !(Result result)
		{
			return !result.Success;
		}

		public static bool operator true(Result result)
		{
			return result.Success;
		}

		public static bool operator false(Result result)
		{
			return !result.Success;
		}

		public override string ToString()
		{
			return Message.ToString();
		}
	}
}