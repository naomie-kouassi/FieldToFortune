namespace FieldToFortune.Model;

public class Market
{
    public List<Commodity> Commodities { get; set; } = new();
    public const double RiskFreeRate = 0.05;
    public string? LastNews { get; set; }


    public void InitializeMarket()
    {
        List<Commodity> commodities =
        [
            new ("Wheat", 95, 0.03),
            new ("Corn", 150, 0.05),
            new ("Orange Juice", 180, 0.08),
            new ("Coffee", 200, 0.1),
            new ("Cocoa", 320, 0.16)
        ];
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
            var newPrice = priceProvider.GetPrice(commodity.Name,turn);
            commodity.SetPrice(newPrice);
        }
    }

    private MarketNews? GenerateNews(int turn, IPriceProvider priceProvider)
    {
        //A piece of news is generated on a 3-turns rotation, except for the last one
        if (turn % 3 != 2 || turn+1>GameState.NbTurns) return null;
        return new MarketNews(this, turn, priceProvider);
    }
    
}
