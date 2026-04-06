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
            ["Due to an intense drought, prices of "," are expected to skyrocket."],
            ["Due to a frost episode, prices of "," are expected to increase significantly."]
        ];

        string[][] priceDecreaseStories =
        [
            ["Due to a good harvest, prices of "," are expected to plummet."]
        ];
        
        if (priceIncrease) return priceIncreaseStories[_random.Next(priceIncreaseStories.Length)];
        
        return priceDecreaseStories[_random.Next(priceDecreaseStories.Length)];
    }

    private NewsElements ChooseNews(Market market, int turn, IPriceProvider priceProvider)
    {
        if (_random.NextDouble() < NewsReliability) return RealNews(market, turn, priceProvider);
        
        int randomIndex = _random.Next(market.Commodities.Count);
        var commodity = market.GetCommodity(randomIndex);
        var priceIncrease = _random.NextDouble() < 0.5;
        
        return new NewsElements(commodity, priceIncrease);
    }

    //News about the commodity that will have the biggest variation next turn
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
        
        Console.WriteLine("News is true!");
        
        var targetCommodity = market.GetCommodity(targetCommodityName);

        return new NewsElements(targetCommodity, princeIncrease);
    }

    public override string ToString()
    {
        return _firstHalf + _newsElements.Commodity.Name + _secondHalf;
    }

    private record NewsElements(Commodity Commodity, bool PriceIncrease);
}