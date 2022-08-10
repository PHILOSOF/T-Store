using TransactionsCreater.Model;

namespace TransactionsCreater.HelperCsv;

public class AccountReader
{
    public Dictionary<string, List<Account>> ReadFile(string filename)
    {
        var accounts = new List<Account>();
        using (StreamReader sr = new StreamReader(filename))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                var account = new Account();
                account.CreatePiece(line);
                accounts.Add(account);
            }
        }
        var accountsDictionary = accounts.GroupBy(a => a.LeadId).ToDictionary(d => d.Key, d => d.ToList());

        return accountsDictionary;
    }

    

}
