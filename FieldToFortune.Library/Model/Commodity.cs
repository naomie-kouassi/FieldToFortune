using System.Diagnostics;

namespace FieldToFortune.Model;

public class Commodity
{
    public string Name { get; }
    public double Price { get; private set; }
    public double Variation  { get; private set; }
    public double Volatility { get; }
    public const double MeanRegressionDegree = 0.2; //for prices simulation
    public bool IsUnlocked { get; set; } = true;
    public double UnlockThreshold = 0;

    public Commodity(string name, double price, double volatility)
    {
        Name = name;
        Price = price; 
        Volatility = volatility;
        Variation = 0;
    }

    public void SetPrice(double newPrice)
    {
        Debug.Assert(newPrice>0);
        Variation = (newPrice - Price) / Price;
        Price = newPrice;
    }
    

    public override string ToString()
    {
      return $"{Name} (Price: {Price:F2}$, Variation: {Variation*100:+0.00;-0.00;+0.00}%)";  
    }
}