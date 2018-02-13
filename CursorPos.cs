namespace KsWare {

	internal class CursorPos {
		/// <summary>
		/// Gets or sets the stored cursor top position.
		/// </summary>
		/// <value>The stored cursor top.</value>
		public int Top { get; set; }

		/// <summary>
		/// Gets or sets the stored cursor left position.
		/// </summary>
		/// <value>The stored cursor left.</value>
		public int Left { get; set; }

		public int BufferWidth { get; set; }

		public int BufferHeight { get; set; }

		public void Save() {
			Left         = System.Console.CursorLeft;
			Top          = System.Console.CursorTop;
			BufferWidth  = System.Console.BufferWidth;
			BufferHeight = System.Console.BufferHeight;
		}

		public void Restore() {
			System.Console.CursorLeft = Left;
			System.Console.CursorTop  = Top;
		}
	}

}