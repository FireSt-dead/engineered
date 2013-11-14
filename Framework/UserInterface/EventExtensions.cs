using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserInterface
{
	public static class EventExtensions
	{
		public static void RaiseRouted<T>(this Event<T> @event, T args, Visual source)
		{
			while (source != null)
			{
				@event.Raise(source, args);
				source = source.Parent;
			}
		}
	}
}
