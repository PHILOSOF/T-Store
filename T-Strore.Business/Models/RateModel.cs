namespace T_Strore.Business.Models;

public static class RateModel
{
    public static Dictionary<(string, string), decimal> currencyRates { get; set; }
    public static string baseCurrency { get; set; }
}
