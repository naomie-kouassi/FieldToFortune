namespace FieldToFortune.Model;

public class PlayerStats
{
    public List<Transaction> TransactionHistory { get; set; } = [];
    public List<double> NetWorthHistory { get; set; }
    public List<double> NetWorthVariationHistory { get; set; }
    
    public const double StartCash  = 5000;

    public PlayerStats()
    {
        NetWorthHistory = [StartCash];
        NetWorthVariationHistory = [0];
    }
    
    
    public void AddTransaction(Transaction t) => TransactionHistory.Add(t);
    
    
    // A single method that returns all the "Live" data at once
    public FinancialSnapshot GetSnapshot(Player player, Market market)
    {
        double netWorth = player.Cash + CalculatePortfolioValue(player, market);
        double profits = netWorth - StartCash;
        double progression = profits / StartCash;
        
        return new FinancialSnapshot(netWorth, profits, progression);
    }
    
    
    public double NetWorthVariation => NetWorthVariationHistory.LastOrDefault();
    
    public int TradeCount => TransactionHistory.Count(t => 
        t.Type == TransactionType.Purchase || 
        t.Type == TransactionType.Sale || 
        t.Type == TransactionType.CallExercise);
    
    public static double CalculateAverageReturn(double progression, int currentTurn)
    {
        if (currentTurn <= 1) return 0;
        return (progression * 100) / (currentTurn - 1);
    }
    
    public static double CalculateProgressToTarget(double currentNetWorth, double targetNetWorth)
    {
        double target = targetNetWorth - StartCash;
        if (target <= 0) return 1; 
        
        double ratio = (currentNetWorth - StartCash) / target;
        return Math.Clamp(ratio, 0, 1);
    }

    public string BestTurn
    {
        get
        {
            if (NetWorthVariationHistory.Count <= 1) return "─";
            
            var bestVariation = NetWorthVariationHistory.Any()? NetWorthVariationHistory.Max():0;
            if (bestVariation <= 0) return "─";
            
            var indexOfBest = NetWorthVariationHistory.IndexOf(bestVariation);
            return $"Turn {indexOfBest} (+{bestVariation * 100:F1}%)";
        }
    }
    
    

    public void UpdateNetWorthHistory(Player player, Market market)
    {
        var netWorth = GetSnapshot(player, market).NetWorth;
        NetWorthHistory.Add(netWorth);
        var i = NetWorthHistory.Count - 1;
        
        if (i <= 0) return;
        
        var previous = NetWorthHistory[i - 1];
        var variation = previous != 0 ? (NetWorthHistory[i] - previous) / previous : 0;
        NetWorthVariationHistory.Add(variation);
    }
    
    
    public double CalculatePortfolioValue(Player player, Market market)
    {
        double value = 0;

        foreach (var (commodityName, quantity) in player.Portfolio.Holdings)
        {
            var commodity = market.GetCommodity(commodityName);
            value += quantity * commodity.Price;
        }

        return value;
    }


    // Simple record for the data transfer
    public record FinancialSnapshot(double NetWorth, double Profits, double Progression);
}