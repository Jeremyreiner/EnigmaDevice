namespace EnigmaSimulator.Extensions
{
    static class EnigmaExtensions
    {
        public static string RemoveSpaceAndToUpper(this string input)
        {
            input = input.ToUpperInvariant();
            input = input.Replace(" ", "");

            return input;
        }

        public static string ConvertToString(this IEnumerable<char> charArr)
        {
            return new(charArr.ToArray());
        }


    }
}