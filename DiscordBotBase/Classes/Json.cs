using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.DiscordBotBase.Classes
{
    /// <summary>
    /// Class to work with Json's.
    /// </summary>
    public static class Json
    {
        /// <summary>
        /// Converts a string Json to <see cref="JObject"/>.
        /// </summary>
        /// <param name="json">Json in string form.</param>
        /// <returns>A <see cref="JObject"/> with the converted Json.</returns>
        public static JObject ToJObject(string json)
            => !string.IsNullOrWhiteSpace(json) ? JsonConvert.DeserializeObject<JObject>(json) : throw new ArgumentNullException("The json can't be null!");

    }
}