
using System.Text;
using T_Strore.Data;
using TransactionsCreater.Model;

namespace TransactionsCreater.HelperCsv;

public class TransactionsToCsv
{
    public void GoToCsv(List<TransactionDtoToCsv> transactions)
    {
        using (StreamWriter file = new StreamWriter(@"E:\sqlTestFiles\TestTo.txt"))
        {
            foreach (var item in transactions)
            {
                file.WriteLine(item.ToCSVRow());
            }
        }
    }

}
