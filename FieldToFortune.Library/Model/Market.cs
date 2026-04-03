namespace FieldToFortune.Model;

public class Market
{
    public List<Commodity> Commodities { get; set; } 
    public const double RiskFreeRate = 0.05;
    public MarketNews? LastNews { get; private set; }

    public Market()
    {
        Commodities = new List<Commodity>();
    }

    public Market(List<Commodity> commodities)
    {
        this.Commodities = commodities;
    }

    public void AddCommodity(Commodity commodity)
    {
        Commodities.Add(commodity);
    }

    public Commodity GetCommodity(int index) => Commodities[index];
    

    public MarketNews? RefreshMarket(int turn, IPriceProvider priceProvider)
    {
        ApplyNewPrices(turn, priceProvider);
        LastNews = GenerateNews(turn, priceProvider);
        return LastNews;
    }

    private void ApplyNewPrices(int turn, IPriceProvider priceProvider)
    {
        foreach (var commodity in Commodities)
        {
            double newPrice = priceProvider.GetPrice(commodity,turn);
            commodity.SetPrice(newPrice);
        }
    }

    private MarketNews? GenerateNews(int turn, IPriceProvider priceProvider)
    {
        //A piece of news is generated on a 3-turns rotation, expct for the last one
        if (turn % 3 != 2 || turn+1>GameState.NbTurns) return null;
        return new MarketNews(this, turn, priceProvider);
    }
    
}
