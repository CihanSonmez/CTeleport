namespace CTeleport.Localization
{
    internal static class StringExtensions
    {
        public static string Format(this string text, params object[] args)
        {
            return string.Format(text, args);
        }
    }
}
