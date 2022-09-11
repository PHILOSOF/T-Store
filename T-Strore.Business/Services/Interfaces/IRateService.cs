using System.Collections.Concurrent;

namespace T_Strore.Business.Services.Interfaces;

public interface IRateService
{
    public void SaveCurrencyRate(Dictionary<string, decimal> rates);
    public Dictionary<string, decimal> GetRate();
    public decimal GetCrossCurrencyRate(string currencyFirst, string currencySecond);
}
