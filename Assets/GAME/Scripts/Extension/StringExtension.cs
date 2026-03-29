namespace Extensions
{
    public static class StringExtension
    {
        public static string Formatted(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
    }

    public static class NumberFormatter
    {
        public static string GetFormatedNumber(int amount)
        {
            return amount switch
            {
                >= 100000000 => (amount / 1000000D).ToString("0.#M"),
                >= 1000000 => (amount / 1000000D).ToString("0.##M"),
                >= 100000 => (amount / 1000D).ToString("0.#k"),
                >= 10000 => (amount / 1000D).ToString("0.##k"),
                _ => amount.ToString()
            };
        }
    }
}
