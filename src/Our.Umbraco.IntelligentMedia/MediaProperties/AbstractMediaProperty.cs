using Umbraco.Core.Models;

namespace Our.Umbraco.IntelligentMedia.MediaProperties
{
	public abstract class AbstractMediaProperty
	{
		private IMedia _media;
		private string _propertyName;

		public AbstractMediaProperty(IMedia media, string propertyName)
		{
			_media = media;
			_propertyName = propertyName;
		}

		public void SetValueIfExists(object value)
		{
			if (_media.HasProperty(_propertyName))
			{
				_media.SetValue(_propertyName, value);
			}
		}

		public T Value<T>()
		{
			return _media.GetValue<T>(_propertyName);
		}

		public bool Exists()
		{
			return _media.HasProperty(_propertyName);
		}
	}
}
