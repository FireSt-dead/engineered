using System.Collections.Generic;

namespace UserInterface.Keyboard
{
	public class KeyboardNotifications
	{
		private IList<Visual> focusedElements;
		private Visual target;

		public KeyboardNotifications()
		{
			this.focusedElements = new List<Visual>();
		}

		public Visual Root { get; set; }

		public void KeyboardTarget(Visual target)
		{
			this.target = target;
		}

		public void UpdateKeyboard()
		{
			var newFocusedElements = new List<Visual>();

			var focused = target;
			while (focused != null)
			{
				newFocusedElements.Add(focused);
				focused = focused.Parent;
			}

			if (newFocusedElements.Count == 0 || newFocusedElements[newFocusedElements.Count - 1] != this.Root)
			{
				newFocusedElements.Clear();
			}

			int oldDifferenceIndex = focusedElements.Count - 1;
			int newDifferenceIndex = newFocusedElements.Count - 1;

			while (oldDifferenceIndex >= 0 && newDifferenceIndex >= 0 && focusedElements[oldDifferenceIndex] == newFocusedElements[newDifferenceIndex])
			{
				oldDifferenceIndex--;
				newDifferenceIndex--;
			}

			// Raise Leave events
			for (int i = 0; i <= oldDifferenceIndex; i++)
			{
				var v = focusedElements[i];
				KeyboardEvents.Leave.Raise(v, null);
			}

			// Raise Enter event
			for (int i = newDifferenceIndex; i >= 0; i--)
			{
				var v = newFocusedElements[i];
				KeyboardEvents.Enter.Raise(v, null);
			}

			this.focusedElements = newFocusedElements;
		}

		public void Char(char c)
		{
			RaiseOnFocusedElements(KeyboardEvents.Char, new KeyboardEvents() { InputChar = c });
		}

		public void KeyDown(uint keyCode)
		{
			RaiseOnFocusedElements(KeyboardEvents.KeyDown, new KeyboardEvents() { KeyCode = keyCode });
		}

		public void KeyUp(uint keyCode)
		{
			RaiseOnFocusedElements(KeyboardEvents.KeyUp, new KeyboardEvents() { KeyCode = keyCode });
		}

		private void RaiseOnFocusedElements(Event<KeyboardEvents> @event, KeyboardEvents args)
		{
			foreach (var child in this.focusedElements)
			{
				@event.Raise(child, args);
			}
		}
	}
}
