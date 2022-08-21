using System.Globalization;
using T_Strore.Data;

namespace FakeTransactionsCreater.Model
{
    public class TransactionCsvToModel : TransactionDto
    {
        public TransactionCsvToModel(string line)
        {
            string[] parts = line.Split(';');
            AccountId = Int64.Parse(parts[1]);
            Date = DateTime.Parse(parts[2]);
            TransactionType = (TransactionType)int.Parse(parts[3]);
            Amount = decimal.Parse(parts[4], CultureInfo.InvariantCulture);
            Currency = (Currency)int.Parse(parts[5]);
        }
    }
}
