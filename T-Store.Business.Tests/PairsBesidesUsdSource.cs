
using System.Collections;
using T_Strore.Data;

namespace T_Store.Business.Tests;

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
    };

    public IEnumerator GetEnumerator()
    {
        foreach(Currency curFirst in currencies)
        {

            foreach(Currency curSecond in currencies)
            {
                yield return new object[]
                {
                     curFirst,curSecond
                };
            }
        }       
    }
}




