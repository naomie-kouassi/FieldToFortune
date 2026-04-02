namespace FieldToFortune.Model;

public class Player
{
    public String Name { get; private set; }
    public double Cash { get; set; }
    public Portfolio Portfolio { get; }
    public Dictionary<Call,int> Calls { get; }
    public List<Transaction> TransactionHistory { get; }

    public const int StartCash = 1000;

    public Player(String name)
    {
        Name = name;
        Portfolio = new Portfolio();
        Cash = StartCash;
        Calls = new Dictionary<Call, int>();
        TransactionHistory = [];
    }

    public double NetWorth => Cash + Portfolio.Value();
    public double NetWorthChange => (NetWorth - StartCash) / StartCash;

    public void AddCall(Call call, int quantity) => Calls[call] = quantity;
    public void RemoveCall(Call call) => Calls.Remove(call);
    
    public void AddTransaction(Transaction t) => TransactionHistory.Add(t);

}