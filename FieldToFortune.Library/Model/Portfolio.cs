using System.Text;

namespace FieldToFortune.Model;


public class Portfolio
{
    public Dictionary<Commodity, int> Commodities { get; }
    
    public Portfolio()
    {
        Commodities = new Dictionary<Commodity, int>();
    }

    public int Quantity(Commodity commodity) => Commodities[commodity];

    public void AddCommodity(Commodity commodity, int quantity)
    {
        Commodities[commodity] = Commodities.GetValueOrDefault(commodity) + quantity;
    }

    public void RemoveCommodity(Commodity commodity, int quantity)
    {
        int current = Commodities.GetValueOrDefault(commodity);

        if (quantity > current)
        {
            return;
        }
        
        Commodities[commodity]  = Commodities.GetValueOrDefault(commodity) - quantity;
    }

    public double Value()
    {
        double value = 0;

        foreach (Commodity commodity in Commodities.Keys)
        {
            value+= Commodities[commodity] * commodity.Price;
        }

        return value;
    }

    public bool IsEmpty()
    {
        foreach (Commodity commodity in Commodities.Keys)
        {
            if (Commodities[commodity] != 0) return false;
        }
        return true;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        var i = 1;

        foreach (Commodity commodity in Commodities.Keys)
        {
            sb.Append($"{i}. x{Commodities[commodity]} {commodity} \n");
            i++;
        }
        
        sb.Append($"\nTotal Value: {Value():F2}$");
        
        return sb.ToString();
    }
    
}