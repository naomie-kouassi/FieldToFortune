namespace FieldToFortune.Model;

public class SimulatedPriceProvider:IPriceProvider
{
    public Dictionary<string,double[]> _prices {get; set; }
    private readonly Random _random = new();
    
    public SimulatedPriceProvider() 
    { 
        _prices = new Dictionary<string, double[]>();
    }

    public double GetPrice(string commodityName, int turn)
    {
        return _prices[commodityName][turn];
    }

    public double[] GetFirstPrices(string commodityName, int lastTurn)
    {
        return _prices[commodityName][..lastTurn];
    }

    public Dictionary<string, double[]> GetAllPrices()
    {
        return _prices;
    }

    public Dictionary<string, double> GetAllPrices(int turn)
    {
        var allPrices = new Dictionary<string, double>();
        foreach (var commodityName in _prices.Keys) allPrices[commodityName] = _prices[commodityName][turn];
        return allPrices;
    }
    
    
    
    public SimulatedPriceProvider(Market market)
    {
        _prices = new Dictionary<string, double[]>();
        foreach (var commodity in market.Commodities)
        {
            _prices[commodity.Name] = SimulatePrices(commodity);
        }
    }

    private double[] SimulatePrices(Commodity commodity)
    {
        double[] prices = new double[GameState.NbTurns + 1];
        prices[0] = commodity.Price;

        double longTermMean = commodity.Price;
        double kappa = Commodity.MeanRegressionDegree;
        double sigma = commodity.Volatility;
        double dt = 1;


        for (int i=0; i < GameState.NbTurns; i++)
        {
            double Z =Gaussian();
            double logPrice = Math.Log(prices[i]);
            double logMean = Math.Log(longTermMean);
            double newLogPrice = logPrice + kappa * (logMean - logPrice) * dt 
                                          + sigma * Z;
            prices[i+1] = Math.Exp(newLogPrice);

        }

        return prices;
    }
    
    private double Gaussian()
    {
        double x1 = _random.NextDouble(); //never equal to 1 (cf NextDouble documentation)
        double x2 = _random.NextDouble();

        return Math.Sqrt(-2.0 * Math.Log(x1)) *
               Math.Sin(2.0 * Math.PI * x2);
    }
    
}