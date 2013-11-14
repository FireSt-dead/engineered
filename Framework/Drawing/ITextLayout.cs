using Geometry;
using System.Collections.Generic;

namespace Drawing
{
	public interface ITextLayout
	{
		Rect BoundingBox { get; }
		CharacterLayout HitTestCharacter(uint index, CharacterSide side);
		CharacterHit HitTestPoint(Point position);
        IEnumerable<Rect> HitTestRange(uint index, uint length);
        void SetForeground(IBrush foreground, uint index, uint length);
	}
}
