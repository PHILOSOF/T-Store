
using System.Text;
using T_Strore.Data;
using TransactionsCreater.Model;

namespace TransactionsCreater.HelperCsv;

public class TransactionsToCsv
{
    public void ConvertToCsv(List<TransactionDtoToCsv> transactions,string filename)
    {
        using (var file = new StreamWriter(filename))
        {
            foreach (var item in transactions)
            {
                file.WriteLine(item.ToCsvRow());
            }
        }
    }
}
