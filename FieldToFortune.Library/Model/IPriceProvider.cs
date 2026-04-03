namespace FieldToFortune.Model;

public interface IPriceProvider
{
    public double GetPrice(Commodity commodity, int turn);
    public double[] GetFirstPrices(Commodity commodity, int lastTurn);
    public Dictionary<Commodity,double[]> GetAllPrices();
    public Dictionary<Commodity, double> GetAllPrices(int turn);
}