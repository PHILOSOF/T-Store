using TransactionsCreater.Model;

namespace TransactionsCreater.HelperCsv;

internal class AccountReader
{
    internal Dictionary<int, List<Account>> GetDictionaryOut(string filename)
    {
        string line;
        var accounts = new List<Account>();

        using (StreamReader sr = new StreamReader(filename))
        {
            while ((line = sr.ReadLine()) != null)
            {
                var account = new Account(line);
                accounts.Add(account);
            }
        }
        var accountsDictionary = accounts.GroupBy(a => a.LeadId).ToDictionary(d => d.Key, d => d.ToList());

        return accountsDictionary;
    }

    

}
