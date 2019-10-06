using Newtonsoft.Json.Linq;

namespace Raindream.Utils
{
    class ConfigUtils
    {
        public static string rawConfig;

        public static JObject config;

        public ConfigUtils()
        {
            rawConfig = System.IO.File.ReadAllText("../config.json");
            config = JObject.Parse(rawConfig);
        }
    }
}