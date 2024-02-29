namespace ContactsManager.UI.Utilities
{
    public static class InputSanitizer
    {
        /// <summary>
        /// Sanitizes the input by removing any occurrences of Environment.NewLine.
        /// </summary>
        /// <param name="input">The input string to be sanitized.</param>
        /// <returns>The sanitized input string.</returns>
        public static string SanitizeInput(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            else
            {
                return input.Replace(Environment.NewLine, "");
            }
        }
    }
}
