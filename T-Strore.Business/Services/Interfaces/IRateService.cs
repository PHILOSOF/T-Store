using System.Collections.Concurrent;

namespace T_Strore.Business.Services.Interfaces;

public interface IRateService
{
    public Dictionary<(string, string), decimal> currencyRates { get; set; }
    public void SaveCurrencyRate(Dictionary<string, decimal> rates);

    public Dictionary<(string, string), decimal> GetRate();
}
