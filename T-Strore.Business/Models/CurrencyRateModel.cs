using System.Collections.Concurrent;

namespace T_Strore.Business.Models;

public static class CurrencyRateModel
{
    public static ConcurrentDictionary<string, decimal> CurrencyRate { get; set; }
}
