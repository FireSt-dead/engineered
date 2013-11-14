using System;
using System.Collections.Generic;

namespace UserInterface
{
	public sealed class Event<TArgs>
	{
		public object Target { get; private set; }

		private object annotationKey = new object();

		public void AddHandler(IAnnotatable annotatable, Action<TArgs> handler)
		{
			List<Action<TArgs>> events;
			object list;
			if (annotatable.TryGetAnnotation(this.annotationKey, out list))
			{
				events = (List<Action<TArgs>>)list;
			}
			else
			{
				events = new List<Action<TArgs>>();
				annotatable.SetAnnotation(this.annotationKey, events);
			}

			events.Add(handler);
		}

		public void RemoveHandler(IAnnotatable annotatable, Action<TArgs> handler)
		{
			List<Action<TArgs>> events;
			object list;
			if (annotatable.TryGetAnnotation(this.annotationKey, out list))
			{
				events = (List<Action<TArgs>>)list;
				events.Remove(handler);

				if (events.Count == 0)
				{
					annotatable.RemoveAnnotation(this.annotationKey);
				}
			}
		}

		public void Raise(IAnnotatable annotatable, TArgs args)
		{
			List<Action<TArgs>> events;
			object list;
			if (annotatable.TryGetAnnotation(this.annotationKey, out list))
			{
				events = (List<Action<TArgs>>)list;
				this.Target = annotatable;
				foreach (var handler in events)
				{
					handler(args);
				}
			}
		}
	}
}
