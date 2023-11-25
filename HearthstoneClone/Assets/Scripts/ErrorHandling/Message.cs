namespace ErrorHandling
{
	/// <summary>
	/// Used to format multiple error messages in a single <see cref="string"/>.
	/// </summary>
	public class Message
	{
		private string content;

		public Message(string content = "")
		{
			this.content = content;
		}

		public static Message operator +(Message a, string addition)
		{
			return new Message
			{
				content = a.content + " - " + addition
			};
		}

		public static Message operator +(Message a, Message b)
		{
			return new Message
			{
				content = a.content + " | " + b.content,
			};
		}

		public override string ToString()
		{
			return content;
		}
	}
}