using IncredibleBackendContracts.Enums;
using System.Collections;

namespace T_Store.Business.Tests.CaseSource;

public class PairsWithUsdSource : IEnumerable
{
    List<Currency> currencies = new()
    {
        Currency.EUR,
        Currency.RUB,
        Currency.JPY,
        Currency.AMD,
        Currency.BGN,
        Currency.RSD,
        Currency.CNY,
    };

    public IEnumerator GetEnumerator()
    {
        foreach (var currency in currencies)
        {
            yield return new object[]
            {
                currency    
            };
        }
    }
}