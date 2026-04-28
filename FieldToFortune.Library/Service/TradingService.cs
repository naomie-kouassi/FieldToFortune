using System.Text;
using FieldToFortune.Model;

namespace FieldToFortune.Service;

public class TradingService
{
    public Transaction? BuyCommodity(Player player, Commodity commodity, int quantity, int turn)
    {
        if (quantity<=0) return null;

        var cost = commodity.Price * quantity;
        if (player.Cash<cost) return null;
        
        //Update player situation
        player.Cash -= cost;
        player.Portfolio.AddCommodity(commodity.Name, quantity);
        
        //Create transaction for player history
        StringBuilder description = new($"Purchased {quantity}x {commodity.Name} for ${commodity.Price:F2}");
        if (quantity > 1) description.Append(" each");

        var t = new Transaction(TransactionType.Purchase, description.ToString(), -cost, turn);
        player.Stats.AddTransaction(t);
        
        return t;
    }

    public Transaction? SellCommodity(Player player, Commodity commodity, int quantity, int turn)
    {
        if (quantity <= 0) return null;

        var availableQuantity = player.Portfolio.Quantity(commodity.Name);
        if (quantity > availableQuantity) return null;
        
        var gains = commodity.Price*quantity;
        //Update player situation
        player.Cash += gains;
        player.Portfolio.RemoveCommodity(commodity.Name, quantity);
        
        //Create transaction for player history
        StringBuilder description = new($"Sold {quantity}x {commodity.Name} for ${commodity.Price:F2}");
        if (quantity > 1) description.Append(" each");
        
        var t = new Transaction(TransactionType.Sale, description.ToString(), gains, turn);
        player.Stats.AddTransaction(t);

        return t;
    }

    public Transaction? BuyCall(Player player, Commodity commodity, double strikePrice, int expiry, int quantity, int turn)
    {
        if (quantity <= 0 || strikePrice<=0 || expiry<=0) return null;
        
        var premium = ComputePremium(commodity,strikePrice, expiry, quantity);
        if  (player.Cash<premium) return null;
        
        //Update player
        player.Cash -= premium;
        var call = new Call(commodity, strikePrice, expiry, quantity);
        player.AddCall(call);

        //Create transaction for player history
        var description = $"Bought CALL {quantity}x {call.Underlying.Name} (Strike: {call.StrikePrice:F2} exp. T+{call.Expiry})";
        var t = new Transaction(TransactionType.CallPremium, description, -premium, turn);
        player.Stats.AddTransaction(t);
        
        return t;
    }

    public Transaction? ExerciseCall(Player player, Call call, Market market, int turn)
    {
        var nbCommodities = call.Quantity;
        var payoff = call.Payoff;
        var playerNetWorth = player.Stats.GetSnapshot(player, market).NetWorth;
        
        if (playerNetWorth<call.ExerciseCost) return null;
        
        //Update player
        player.RemoveCall(call);
        player.Cash += payoff;
        player.HasExercisableCalls = false;

        //Create transaction for player history
        var description =
            $"Exercised CALL {nbCommodities}x {call.Underlying.Name} @ {call.StrikePrice:F2} (Market: {call.Underlying.Price:F2})";
        
        var t = new Transaction(TransactionType.CallExercise, description, payoff, turn);
        player.Stats.AddTransaction(t);

        return t;

    }
    
    
    
    //Price of a call computed with Black-Scholes formula
    public double ComputePremium(Commodity underlying, double strikePrice, int expiry, int quantity) {
        double S = underlying.Price;
        double K = strikePrice;
        int T = expiry;
        double r = Market.RiskFreeRate;
        double sigma = underlying.Volatility;
        
        double d1 = (Math.Log(S/K) + (r+sigma*sigma/2)*T) / sigma*Math.Sqrt(T);
        double d2 = d1 - sigma*Math.Sqrt(T);

        double C = S * CumulativeNormal(d1) - K * Math.Exp(-r * T) * CumulativeNormal(d2);

        return quantity * C;
        
    }
    
    
    private static double CumulativeNormal(double x)
    {
        return 0.5 * (1.0 + Erf(x / Math.Sqrt(2.0)));
    }

    private static double Erf(double x)
    {
        // Abramowitz and Stegun approximation
        double t = 1.0 / (1.0 + 0.3275911 * Math.Abs(x));
        double poly = t * (0.254829592 + t * (-0.284496736 
                                              + t * (1.421413741 + t * (-1.453152027 
                                                                        + t * 1.061405429))));
        double result = 1.0 - poly * Math.Exp(-x * x);
        return x >= 0 ? result : -result;
    }
    
}