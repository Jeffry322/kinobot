namespace KinoBot.API.Utils;

public static class CodeToFlagConvertor
{
    public static string ToFlag(string countryCode)
    {
        if (string.IsNullOrEmpty(countryCode) || countryCode.Length != 2)
            return string.Empty;
    
        countryCode = countryCode.ToUpper();
    
        // Regional Indicator Symbol Letter A starts at U+1F1E6
        // Convert each letter to its corresponding regional indicator symbol
        return string.Concat(countryCode.Select(c => 
            char.ConvertFromUtf32(0x1F1E6 + c - 'A')));
    }
}