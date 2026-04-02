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
        player.Portfolio.AddCommodity(commodity, quantity);

        var description = $"Purchased {commodity.Name} {quantity}x";

        var t = new Transaction(TransactionType.Purchase, description, -cost, turn);
        player.AddTransaction(t);
        
        return t;
    }

    public Transaction? SellCommodity(Player player, Commodity commodity, int quantity, int turn)
    {
        if (quantity <= 0) return null;

        var availableQuantity = player.Portfolio.Quantity(commodity);
        if (quantity > availableQuantity) return null;
        
        var gains = commodity.Price*quantity;
        //Update player situation
        player.Cash += gains;
        player.Portfolio.RemoveCommodity(commodity, quantity);
        
        var description = $"Sold {commodity.Name} {quantity}x";
        
        var t = new Transaction(TransactionType.Sale, description, gains, turn);
        player.AddTransaction(t);

        return t;
    }

    public Transaction? BuyCall(Player player, Commodity commodity, double strikePrice, int expiry, int quantity, int turn)
    {
        if (quantity <= 0 || strikePrice<=0 || expiry<=0) return null;
        
        var premium = quantity*ComputePremium(commodity,strikePrice, expiry);
        if  (player.Cash<premium) return null;
        
        //Update player
        player.Cash -= premium;
        var call = new Call(commodity, strikePrice, expiry);
        player.AddCall(call, quantity);

        var description = $"Bought {quantity}x {call.Underlying.Name} Call (Strike: {call.StrikePrice:F2} exp. T+{call.Expiry})";
        var t = new Transaction(TransactionType.CallContract, description, -premium, turn);
        player.AddTransaction(t);
        
        return t;
    }

    public void ExerciseCall(Player player, Call call, int turn)
    {
        var nbCalls = player.Calls[call];
        var totalPayoff = nbCalls * call.Payoff;
        
        player.RemoveCall(call);
        if (totalPayoff<=0) return;

        player.Cash += totalPayoff;

        var description =
            $"Sold {nbCalls}x {call.Underlying.Name} @ {call.StrikePrice:F2} (Market: {nbCalls*call.Underlying.Price:F2})";
        
        var t = new Transaction(TransactionType.CallExercise, description, totalPayoff, turn);
        player.AddTransaction(t);
        
    }
    
    public void ExerciseAllCalls(Player player, int turn)
    {
        foreach (var call in player.Calls.Keys)
        {
            if (call.IsAtExerciceDate) ExerciseCall(player,call, turn);
        }
    }
    
    
    //Price of a call computed with Black-Scholes formula
    public double ComputePremium(Commodity underlying, double strikePrice, int expiry) {
        double S = underlying.Price;
        double K = strikePrice;
        int T = expiry;
        double r = Market.RiskFreeRate;
        double sigma = underlying.Volatility;
        
        double d1 = (Math.Log(S/K) + (r+sigma*sigma/2)*T) / sigma*Math.Sqrt(T);
        double d2 = d1 - sigma*Math.Sqrt(T);

        double C = S * CumulativeNormal(d1) - K * Math.Exp(-r * T) * CumulativeNormal(d2);

        return C;
        
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