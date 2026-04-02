using System.Collections;

namespace FieldToFortune.Model;

public class Market
{
    public List<Commodity> Commodities { get; set; } = [];
    public const double RiskFreeRate = 0.05;

    public void AddCommodity(Commodity commodity)
    {
        Commodities.Add(commodity);
    }

    public Commodity? GetCommodity(int index)
    {
        if (index >= 0 && index < Commodities.Count) return Commodities[index];
        return null;
    }

    public String? RefreshMarket(int turn, IPriceProvider priceProvider)
    {
        ApplyNewPrices(turn, priceProvider);
        return GenerateEvent(turn, priceProvider);

    }

    private void ApplyNewPrices(int turn, IPriceProvider priceProvider)
    {
        foreach (var commodity in Commodities)
        {
            double newPrice = priceProvider.GetPrice(commodity,turn);
            commodity.SetPrice(newPrice);
        }
    }

    private String? GenerateEvent(int turn, IPriceProvider priceProvider)
    {
        return null;
    }
    
}
