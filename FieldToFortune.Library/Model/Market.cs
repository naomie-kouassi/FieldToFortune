using System.Collections.Specialized;

namespace FieldToFortune.Model;

public class Market
{
    public Commodity[] Commodities { get; set; } = [];
    public const double RiskFreeRate = 0.05;
    public string? LastNews { get; set; } = null;
    

    public void InitializeMarket()
    {
        Commodity[] commodities =
        [
            new("Soybeans", 460, 0.048),
            new("Corn", 240, 0.059),
            new("Barley", 160, 0.064),
            new("Bananas", 1330, 0.071),
            new("Sunflower Oil", 1540, 0.077),
            new("Cocoa", 4680, 0.11)
        ];
        
        Commodities = commodities;
    }

    public void InitializeMarket(Dictionary<string, double[]> dictionary)
    {
        Commodity[] commodities = new Commodity[dictionary.Count];
        var i = 0;

        foreach ((string commodityName, double[] prices) in dictionary)
        {
            var commodity = new Commodity(commodityName, prices[0], 1);
            commodities[i] = commodity;
            i++;

        }

        Commodities = commodities;
        LastNews = null;
    }



    public Commodity GetCommodity(int index) => Commodities[index];
    
    public Commodity GetCommodity(string name) => Commodities.First(c => c.Name == name);
    

    public string? RefreshMarket(int turn, IPriceProvider priceProvider)
    {
        ApplyNewPrices(turn, priceProvider);
        MarketNews? news = GenerateNews(turn, priceProvider);
        LastNews = news?.ToString();
        return LastNews;
    }

    private void ApplyNewPrices(int turn, IPriceProvider priceProvider)
    {
        foreach (var commodity in Commodities)
        {
            var newPrice = priceProvider.GetPrice(commodity.Name,turn+1);
            commodity.SetPrice(newPrice);
        }
    }

    private MarketNews? GenerateNews(int turn, IPriceProvider priceProvider)
    {
        //A piece of news is generated on a 3-turns rotation, except for the last one
        if (turn % 3 != 2 || turn+2>GameState.NbTurns) return null;
        return new MarketNews(this, turn, priceProvider);
    }
    
}
