namespace UserInterface
{
	public interface IAnnotatable
	{
		void SetAnnotation(object key, object value);
		bool TryGetAnnotation(object key, out object value);
		void RemoveAnnotation(object key);
	}
}
