namespace TransactionsCreater.Model;

public class Account
{
    public int Id { get; set; }
    public int Currency { get; set; }
    public int LeadId { get; set; }


    public void CreatePiece(string line)
    {
        string[] parts = line.Split(';');
        Id =Int32.Parse(parts[0]);
        Currency = Int32.Parse(parts[1]);
        LeadId = Int32.Parse(parts[3]);

    }
}
