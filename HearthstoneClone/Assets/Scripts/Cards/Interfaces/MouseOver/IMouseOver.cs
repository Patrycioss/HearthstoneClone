namespace Cards.Interfaces.MouseOver
{
	public interface IMouseOver
	{
		public bool IsHovering { get; set; }
		public void OnStartHover();
		public void OnEndHover();
	}
}