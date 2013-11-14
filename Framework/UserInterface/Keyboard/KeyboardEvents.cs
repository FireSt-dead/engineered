namespace UserInterface.Keyboard
{
	public sealed class KeyboardEvents
	{
		public static readonly Event<KeyboardEvents> Enter = new Event<KeyboardEvents>();
		public static readonly Event<KeyboardEvents> Leave = new Event<KeyboardEvents>();

		public static readonly Event<KeyboardEvents> Char = new Event<KeyboardEvents>();

		public static readonly Event<KeyboardEvents> KeyDown = new Event<KeyboardEvents>();
		public static readonly Event<KeyboardEvents> KeyUp = new Event<KeyboardEvents>();

		public static readonly Event<KeyboardEvents> RequestKeyboard = new Event<KeyboardEvents>();

		// TODO: This is used by the RequestKeyboard... Implement routed event instead that will preserve the initial sender / target / source...
		public Visual Source { get; set; }

		public char InputChar { get; set; }
		public uint KeyCode { get; set; }
	}
}
