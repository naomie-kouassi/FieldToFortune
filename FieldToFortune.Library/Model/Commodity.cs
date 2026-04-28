using System.Diagnostics;

namespace FieldToFortune.Model;

public class Commodity
{
    public string Name { get; set; }
    public double Price { get; set; }
    public double Variation  { get; set; }
    public double Volatility { get; set; }
    public const double MeanRegressionDegree = 0.2; //for prices simulation

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

    public int RiskLevel()
    {
        switch (Volatility)
        {
            case <= 0.055 : return 1;
            case <= 0.065 : return 2;
            case <= 0.075 : return 3;
            case <= 0.085: return 4;
            default : return 5;
        }
    }

    public string RiskIndicator()
    {
        switch (RiskLevel())
        {
            case 1: return "●○○○○";
            case 2: return "●●○○○";
            case 3: return "●●●○○";
            case 4: return "●●●●○";
            default: return "●●●●●";
        }
    }
    

    public override string ToString()
    {
      return $"{Name} (Price: {Price:F2}$, Variation: {Variation*100:+0.00;-0.00;+0.00}%)";  
    }
}