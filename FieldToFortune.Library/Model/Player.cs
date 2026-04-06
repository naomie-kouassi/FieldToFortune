using System.Text.Json.Serialization;

namespace FieldToFortune.Model;

public class Player
{
    public String Name { get; set; }
    public double Cash { get; set; }
    public Portfolio Portfolio { get; set; } = new();
    public Dictionary<Call, int> Calls { get; set; } = new();
    public List<Transaction> TransactionHistory { get; set; } = [];
    public List<double> NetWorthHistory { get; set; }
    public List<double> NetWorthVariationHistory { get; set; }

    public readonly double StartCash = 1000;

    [JsonConstructor]
    public Player()
    {
        Name = "";
        NetWorthHistory = [];
        NetWorthVariationHistory = [];
    }

    public Player(string name)
    {
        Name = name;
        Cash = StartCash;
        NetWorthHistory = [StartCash];
        NetWorthVariationHistory = [0];
    }

    public double NetWorth(Market market) => Cash + Portfolio.Value(market);

    public double Progession(Market market) => (NetWorth(market) - StartCash) / StartCash;
    public double Profits(Market market) => NetWorth(market) - StartCash;
    public double NetWorthVariation => NetWorthVariationHistory.Last();

    public void AddCall(Call call, int quantity) => Calls[call] = quantity;
    public void RemoveCall(Call call) => Calls.Remove(call);

    public void UpdateCalls()
    {
        foreach (Call call in Calls.Keys)
        {
            call.Expiry--;
            if(call.Expiry<0) RemoveCall(call);
        }
    }
    
    public void AddTransaction(Transaction t) => TransactionHistory.Add(t);

    public void UpdateNetWorthHistory(Market market)
    {
        NetWorthHistory.Add(NetWorth(market));
        var i = NetWorthHistory.Count - 1;
        if (i > 0)
        {
            var variation = (NetWorthHistory[i] - NetWorthHistory[i-1]) / NetWorthHistory[i-1];
            NetWorthVariationHistory.Add(variation);
        }
    }

}