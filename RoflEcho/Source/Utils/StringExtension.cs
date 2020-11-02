namespace RoflEcho
{
    public static class StringExtension
    {
        public static string Quote(this string str)
        {
            if (str.StartsWith('"') && str.EndsWith('"')) return str;
            return $"\"{str}\"";
        }
    }
}
