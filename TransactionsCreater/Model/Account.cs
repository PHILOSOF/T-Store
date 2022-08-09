namespace TransactionsCreater.Model;

public class Account
{
    public string Id { get; set; }
    public string Currency { get; set; }
    public string LeadId { get; set; }


    public void CreatePiece(string line)
    {
        string[] parts = line.Split(';');
        Id = parts[0];
        Currency = parts[1];
        LeadId = parts[3];

    }
}
