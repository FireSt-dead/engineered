namespace Geometry
{
	public struct Interval
	{
		public float Start;
		public float End;

		public float Length
		{
			get
			{
				return this.End - this.Start;
			}
		}
	}
}
