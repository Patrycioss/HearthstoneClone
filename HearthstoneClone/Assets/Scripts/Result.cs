using JetBrains.Annotations;

/// <summary>
/// Object that can be returned by a function that gets something.
/// </summary>
/// <typeparam name="T">Type of the value to be returned.</typeparam>
public class Result<T> : Result
{
	/// <summary>
	/// The value of the object.
	/// </summary>
	[CanBeNull] public T Value;
}

public class Result
{
	/// <summary>
	/// Did it get the object successfully.
	/// </summary>
	public SuccessType Success = SuccessType.Success;

	/// <summary>
	/// Message that can indicate an error or something else.
	/// </summary>
	public Message Message;
}

public enum SuccessType
{
	Success,
	Problem,
	Failed,
}

public class Message
{
	private string content;

	public Message(string content = "")
	{
		this.content = content;
	}

	public static Message operator +(Message a, string addition)
	{
		return new Message()
		{
			content = a.content + " - " + addition
		};
	}
	
	public static Message operator +(Message a, Message b)
	{
		return new Message()
		{
			content = a.content + " | " + b.content,
		};
	}

	public override string ToString()
	{
		return content;
	}
}