using System.Collections.Concurrent;

namespace T_Strore.Business.Models;

public static class CurrencyRateModel
{
    public static Dictionary<string, decimal> CurrencyRates { get; set; }
}
