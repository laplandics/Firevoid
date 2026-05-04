using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tools
{
    public static class JsonSettingsSetter
    {
        public static void SetSettings()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                Converters = new List<JsonConverter> { new Vector3Converter() }
            };
        }
    }
}