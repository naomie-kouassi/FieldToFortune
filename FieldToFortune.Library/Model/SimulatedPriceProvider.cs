namespace FieldToFortune.Model;

public class SimulatedPriceProvider:IPriceProvider
{
    private readonly Dictionary<Commodity,double[]> _prices;
    private readonly Random _random = new();

    public double GetPrice(Commodity commodity, int turn)
    {
        return _prices[commodity][turn];
    }

    public double[] GetFirstPrices(Commodity commodity, int lastTurn)
    {
        return _prices[commodity][..lastTurn];
    }

    public Dictionary<Commodity, double[]> GetAllPrices()
    {
        return _prices;
    }

    public Dictionary<Commodity, double> GetAllPrices(int turn)
    {
        var allPrices = new Dictionary<Commodity, double>();
        foreach (var commodity in _prices.Keys) allPrices[commodity] = _prices[commodity][turn];
        return allPrices;
    }
    
    
    
    public SimulatedPriceProvider(Market market)
    {
        _prices = new Dictionary<Commodity, double[]>();
        foreach (var commodity in market.Commodities)
        {
            _prices[commodity] = SimulatePrices(commodity);
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