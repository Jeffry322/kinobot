namespace KinoBot.API.Common.Utils;

public static class MoneyFormatter
{
    public static string FormatMoney(int dollars)
    {
        return dollars switch
        {
            >= 1_000_000_000 => FormatValue(dollars / 1_000_000_000m, "billion"),
            >= 1_000_000 => FormatValue(dollars / 1_000_000m, "million"),
            >= 1_000 => FormatValue(dollars / 1_000m, "k"),
            _ => $"${dollars}"
        };
    }

    private static string FormatValue(decimal value, string suffix)
    {
        return value % 1 == 0 
            ? $"${(int)value} {suffix}" 
            : $"${value:0.##} {suffix}";
    }

}