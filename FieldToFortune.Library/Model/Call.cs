namespace FieldToFortune.Model;

public class Call
{
    public Commodity Underlying { get; set; }
    public double StrikePrice { get; set; }
    public int Expiry { get; set; } //nb of turns left before the call expire
    public int Quantity { get; set; } //how many units of the underlying asset the player can buy with the call
    
    public Call(Commodity underlying, double strikePrice,  int expiry, int quantity){
        Underlying = underlying;
        StrikePrice = strikePrice;
        Expiry = expiry;
        Quantity = quantity;
    }
    
    
    public bool IsAtExerciceDate => Expiry==0;
    public bool IsProfitable => Payoff > 0;
    public double Payoff => Quantity * Math.Max(0, Underlying.Price - StrikePrice);
    public double ExerciseCost => StrikePrice * Quantity;
    

    public override string ToString()
    {
        return $"CALL {Underlying.Name} (Strike price: {StrikePrice:F2}$, Expires in: {Expiry} turns)";
    }
    
    
}