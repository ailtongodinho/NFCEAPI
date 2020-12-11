using System.Globalization;
using System.Text;

namespace NFCE.API.Helpers
{
    public static class StringHelper
    {
        public static string RemoveAccents(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }
    }
}