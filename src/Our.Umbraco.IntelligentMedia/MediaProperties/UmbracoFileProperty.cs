using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace Our.Umbraco.IntelligentMedia.MediaProperties
{
	public class UmbracoFileProperty : AbstractMediaProperty
	{
		public UmbracoFileProperty(IMedia media) : base(media, "umbracoFile") { }

        public bool IsImageCropper()
        {
            return Value<object>() is string value && value.DetectIsJson();
        }

        public decimal FocalPointLeft
        {
            set
            {
                if (Value<object>() is string val && val.DetectIsJson())
                {
                    var jObject = JsonConvert.DeserializeObject<JObject>(val);
                    jObject["focalPoint"]["left"] = value;
                    SetValueIfExists(JsonConvert.SerializeObject(jObject));
                }
            }
        }

        public decimal FocalPointTop
        {
            set
            {
                if (Value<object>() is string val && val.DetectIsJson())
                {
                    var jObject = JsonConvert.DeserializeObject<JObject>(val);
                    jObject["focalPoint"]["top"] = value;
                    SetValueIfExists(JsonConvert.SerializeObject(jObject));
                }
            }
        }

        public string Src
        {
            get
            {
                var value = Value<object>() as string;
                if (value != null && value.DetectIsJson())
                {
                    // the property value is a JSON serialized image crop data set - grab the "src" property as the file source
                    var jObject = JsonConvert.DeserializeObject<JObject>(value);
                    value = jObject != null ? jObject.GetValue("src").Value<string>() : value;
                }
                return value;
            }
            set
            {
                var property = Value<object>();
                if (property == null)
                {
                    return;
                }
                var filename = Value<object>() as string;
                if (filename != null && filename.DetectIsJson())
                {
                    var jObject = JsonConvert.DeserializeObject<JObject>(value);
                    jObject.Property("src").Value = value;
                    SetValueIfExists(JsonConvert.SerializeObject(jObject));
                }
                else
                {
                    SetValueIfExists(value);
                }
            }
        }
    }
}
