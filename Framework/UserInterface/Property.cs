namespace UserInterface
{
	public sealed class Property<T>
	{
		private object annotationKey;

		public Property(T @default)
		{
			this.annotationKey = new object();
			this.Default = @default;
		}

		public T Default { get; private set; }

		public void SetValue(IAnnotatable annotatable, T value)
		{
            if (object.Equals(this.Default, value))
            {
                annotatable.RemoveAnnotation(this.annotationKey);
            }
            else
            {
                annotatable.SetAnnotation(this.annotationKey, value);
            }
		}

		public T GetValue(IAnnotatable annotatable)
		{
			object o;
			if (annotatable.TryGetAnnotation(this.annotationKey, out o))
			{
				return (T)o;
			}
			else
			{
				return this.Default;
			}
		}
	}
}
