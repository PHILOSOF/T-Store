
using FakeTransactionsCreater.Model;
using System.Text;
using T_Strore.Data;
using TransactionsCreater.Model;

namespace TransactionsCreater.HelperCsv;

public class TransactionsCsvHelper
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

    public List<TransactionCsvToModel> ConvertToModel(string filename)
    {
        string line;
        var transactionModel = new List<TransactionCsvToModel>();

        using (StreamReader sr = new StreamReader(filename))
        {
            while ((line = sr.ReadLine()) != null)
            {
                var account = new TransactionCsvToModel(line);
                transactionModel.Add(account);
            }
        }
        return transactionModel;
    }
    
}
