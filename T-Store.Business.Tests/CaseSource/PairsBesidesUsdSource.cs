
using IncredibleBackendContracts.Enums;
using System.Collections;

namespace T_Store.Business.Tests.CaseSource;

public class PairsBesidesUsdSource : IEnumerable
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
        foreach (Currency curFirst in currencies)
        {

            foreach (Currency curSecond in currencies)
            {
                if(curFirst != curSecond)
                {
                    yield return new object[]
                    {
                         curFirst,curSecond
                    };
                }
            }
        }
    }
}




