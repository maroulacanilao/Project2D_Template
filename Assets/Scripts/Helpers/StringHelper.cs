using System.Text;

namespace CustomHelpers
{
    public static class StringHelpers
    {
        public static string SplitCamelCase(this string source)
        {
            if (string.IsNullOrEmpty(source)) return source;
            
            StringBuilder sb = new StringBuilder();

            foreach (var c in source) 
            {
                if (char.IsUpper(c)) sb.Append(' ');

                sb.Append(c);
            }

            return sb.ToString();
        }
        
        
    }
}
