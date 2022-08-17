namespace TransactionsCreater.Model;

public class Account
{
    public long Id { get; set; }
    public int Currency { get; set; }
    public long LeadId { get; set; }
    public DateTime LeadRegistrationDate { get; set; }

    public Account(string line)
    {
        string[] parts = line.Split(';');
        Id = Int64.Parse(parts[0]);
        Currency = Int32.Parse(parts[1]);
        LeadId = Int64.Parse(parts[3]);
        LeadRegistrationDate = DateTime.Parse(parts[5]);
    }
}
