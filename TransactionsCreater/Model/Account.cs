namespace TransactionsCreater.Model;

internal class Account
{
    internal int Id { get; set; }
    internal int Currency { get; set; }
    internal int LeadId { get; set; }


    internal Account(string line)
    {
        string[] parts = line.Split(';');
        Id = Int32.Parse(parts[0]);
        Currency = Int32.Parse(parts[1]);
        LeadId = Int32.Parse(parts[3]);
    }

}
