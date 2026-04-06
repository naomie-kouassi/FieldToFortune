namespace FieldToFortune.Model;

public interface IPriceProvider
{
    public double GetPrice(string commodityName, int turn);
    public double[] GetFirstPrices(string commodityName, int lastTurn);
    public Dictionary<string, double[]> GetAllPrices();
    public Dictionary<string, double> GetAllPrices(int turn);
}