using TransactionsCreater.Model;

namespace TransactionsCreater.HelperCsv;

public class AccountReader
{
    public List<Account> ReadFile(string filename)
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
        return accounts;
    }

    public Dictionary<string, List<Account>> GetAccounts(string foldeerString)
    {

        var accounts = new List<Account>();
        var aa = ReadFile(foldeerString);

        for (int i = 0; i <= aa.Count - 1; i++)
        {
            var account = new Account();
            account.Id = aa[i].Id;
            account.Currency = aa[i].Currency;
            account.LeadId = aa[i].LeadId;
            accounts.Add(account);
        }

        var accountsDictionary = accounts.GroupBy(a => a.LeadId).ToDictionary(d => d.Key, d => d.ToList());
        return accountsDictionary;
    }

}
