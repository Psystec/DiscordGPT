using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordGPT.common
{
    public static class Common
    {
        // Helper method to split text into parts of specified maximum length
        public static List<string> SplitIntoParts(string text, int maxLength)
        {
            var parts = new List<string>();
            for (int i = 0; i < text.Length; i += maxLength)
            {
                parts.Add(text.Substring(i, Math.Min(maxLength, text.Length - i)));
            }
            return parts;
        }
    }
}
