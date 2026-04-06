namespace FieldToFortune.Model;

public class Call
{
    public Commodity Underlying { get; set; }
    public readonly double StrikePrice;
    public int Expiry { get; set; } //nb of turns left before the call expire
    
    public Call(Commodity underlying, double strikePrice,  int expiry){
        this.Underlying = underlying;
        this.StrikePrice = strikePrice;
        this.Expiry = expiry;
    }
    
    
    public void UpdateCall() => Expiry--;
    public bool IsAtExerciceDate => Expiry==0;
    public double Payoff => Math.Max(0, Underlying.Price - StrikePrice);
    

    public override string ToString()
    {
        return $"CALL {Underlying.Name} (Strike price: {StrikePrice:F2}$, Expires in: {Expiry} turns)";
    }
    
    
}