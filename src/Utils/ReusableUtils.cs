using System.Collections.Generic;

namespace Raindream.Utils
{
    public class EmoteService {
        public Dictionary<string, string> emotes = new Dictionary<string, string>();

        public EmoteService() {
            emotes.Add("x", "<:IconX:553868311960748044>");
            emotes.Add("this", "<:IconThis:553869005820002324>");
            emotes.Add("success", "<:IconSuccess:553868293614731284>");
            emotes.Add("provide", "<:IconProvide:553870022125027329>");
            emotes.Add("info", "<:IconInfo:553868326581829643>");
        }
    }
}