namespace WebApp.Plumping {
    public static class StringExtensions {
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        public static bool IsNumeric(this string s) {
            float output;
            return float.TryParse(s, out output);
        }
        public static float ToNumber(this string s) {
            float output;
            float.TryParse(s, out output);
            return output;
        }
        public static byte[] ToByteArray(this string str)
        {
            return System.Text.Encoding.ASCII.GetBytes(str);
        }

        public static string ToString(this byte[] bytes)
        {
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}