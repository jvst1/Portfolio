using System.Globalization;
using System.Text;

namespace Portfolio.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string ApenasNumeros(this string s)
        {
            var stringBuilder = new StringBuilder();
            foreach (var c in s)
            {
                if (char.IsNumber(c)) stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }

        public static bool IsNumeric(this string s)
        {
            float _;
            return float.TryParse(s, out _);
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static bool IsNullOrWhiteSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        public static string SemCaracteresEspeciais(this string s)
        {
            /* Troca os caracteres acentuados por não acentuados */
            string[] acentos = { "ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù", "À", "È", "Ì", "Ò", "Ù", "ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "â", "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û" };
            string[] semAcento = { "c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U", "a", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "A", "O", "N", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U" };
            for (var i = 0; i < acentos.Length; i++)
            {
                s = s.Replace(acentos[i], semAcento[i]);
            }
            /* Troca os caracteres especiais da string por "" */
            string[] caracteresEspeciais = { ".", "\\.", ",", "-", ":", "\\(", "\\)", "ª", "\\|", "\\\\", "°" };
            for (var i = 0; i < caracteresEspeciais.Length; i++)
            {
                s = s.Replace(caracteresEspeciais[i], "");
            }
            /* Troca os espaços no início por "" */
            s = s.Replace("^\\s+", "");
            /* Troca os espaços no início por "" */
            s = s.Replace("\\s+$", "");
            /* Troca os espaços duplicados, tabulações e etc por  " " */
            s = s.Replace("\\s+", " ");

            return s;
        }

        /// <summary>
        /// Use the current thread's culture info for conversion
        /// </summary>
        public static string ToTitleCase(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }

        /// <summary>
        /// Overload which uses the culture info with the specified name
        /// </summary>
        public static string ToTitleCase(this string str, string cultureInfoName)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            var cultureInfo = new CultureInfo(cultureInfoName);
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }

        /// <summary>
        /// Overload which uses the specified culture info
        /// </summary>
        public static string ToTitleCase(this string str, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace(str) ? str : cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
        
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }
    }
}
