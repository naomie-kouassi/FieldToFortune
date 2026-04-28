namespace FieldToFortune.Model;

public class MarketNews
{
    private readonly string _firstHalf;
    private readonly string _secondHalf;
    private readonly NewsElements _newsElements;
    private const double NewsReliability = 0.3; //probability a news is true
    private readonly Random _random = new ();

    public MarketNews(Market market, int turn, IPriceProvider priceProvider)
    {
        _newsElements = ChooseNews(market, turn, priceProvider);
        string[] story = ChooseStory(_newsElements.PriceIncrease);
        _firstHalf = story[0];
        _secondHalf = story[1];
    }

    private string[] ChooseStory(bool priceIncrease)
    {
        string[][] priceIncreaseStories =
        [
            ["Initial reports suggest crop yields for ", " may be lower than expected."],
            ["Traders are eyeing potential supply disruptions for ", ", following regional tensions."],
            ["Early forecasts indicate a heatwave could impact the upcoming harvest of ", "."],
            ["Rumors of new export restrictions on ", " are starting to circulate in the market."],
            ["Analysts are warning of a possible inventory shortage for ", " by next month."],
            ["Speculation is growing that a major producer of ", " might reduce their exports."]
        ];

        string[][] priceDecreaseStories =
        [
            ["Favorable weather in major growing zones suggests an abundant supply of ", "."],
            ["Market whispers indicate a major producer is looking to offload stocks of ", "."],
            ["Improved shipping conditions are expected to ease the current backlog of ", "."],
            ["Forecasters are predicting a seasonal dip in the global demand for ", "."],
            ["New data suggests the recent surplus of ", " could weigh on market prices."],
            ["Recent reports indicate production costs for ", " are finally starting to stabilize."]
        ];
        
        if (priceIncrease) return priceIncreaseStories[_random.Next(priceIncreaseStories.Length)];
        
        return priceDecreaseStories[_random.Next(priceDecreaseStories.Length)];
    }

    private NewsElements ChooseNews(Market market, int turn, IPriceProvider priceProvider)
    {
        if (_random.NextDouble() < NewsReliability) return RealNews(market, turn, priceProvider);
        
        return FakeNews(market);
    }

    //Generate news about the commodity that will have the highest absolute variation the next turn
    private NewsElements RealNews(Market market, int turn, IPriceProvider priceProvider)
    {
        var currentPrices = priceProvider.GetAllPrices(turn);
        var futurePrices = priceProvider.GetAllPrices(turn + 1);
        var variations = new Dictionary<string, double > ();

        foreach (var commodity in currentPrices.Keys)
        {
            variations[commodity] = Math.Abs((futurePrices[commodity]-currentPrices[commodity])/currentPrices[commodity]);
        }
        
        var targetCommodityName = variations.MaxBy(kvp => kvp.Value).Key;
        var princeIncrease = futurePrices[targetCommodityName] > currentPrices[targetCommodityName];
        
        var targetCommodity = market.GetCommodity(targetCommodityName);

        return new NewsElements(targetCommodity, princeIncrease);
    }

    //Generate random news about a random commodity of the market
    private NewsElements FakeNews(Market market)
    {
        int randomIndex = _random.Next(market.Commodities.Length);
        var commodity = market.GetCommodity(randomIndex);
        var priceIncrease = _random.NextDouble() < 0.5;
        
        return new NewsElements(commodity, priceIncrease);
    }

    public override string ToString()
    {
        return _firstHalf + _newsElements.Commodity.Name + _secondHalf;
    }

    private record NewsElements(Commodity Commodity, bool PriceIncrease);
}