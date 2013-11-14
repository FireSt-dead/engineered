using System.Collections.Generic;

namespace UserInterface
{
	public class VisualContainer : Composite
	{
		public new IList<Visual> Children
		{
			get
			{
				return base.Children;
			}
		}
	}
}
