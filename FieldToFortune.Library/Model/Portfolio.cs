using System.Text;
using System.Text.Json.Serialization;

namespace FieldToFortune.Model;


public class Portfolio
{
    public Dictionary<string, int> Holdings { get; set; }
    
    [JsonConstructor]
    public Portfolio()
    {
        Holdings = new Dictionary<string, int>();
    }

    public int Quantity(string commodityName)
    {
        if (Holdings.TryGetValue(commodityName, out int qty))
        {
            return qty;
        }
        return 0;
    }

    public void AddCommodity(string commodityName, int quantity)
    {
        Holdings[commodityName] = Holdings.GetValueOrDefault(commodityName) + quantity;
    }

    public void RemoveCommodity(string commodityName, int quantity)
    {
        int current = Holdings.GetValueOrDefault(commodityName);

        if (quantity > current)
        {
            return;
        }
        
        Holdings[commodityName]  = Holdings.GetValueOrDefault(commodityName) - quantity;
    }

    public double Value(Market market)
    {
        double value = 0;

        foreach (string commodityName in Holdings.Keys)
        {
            var price = market.GetCommodity(commodityName).Price;
            value+= Holdings[commodityName] * price;
        }

        return value;
    }
    
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        var i = 1;

        foreach (string commodityName in Holdings.Keys)
        {
            sb.Append($"{i}. x{Holdings[commodityName]} {commodityName} \n");
            i++;
        }
        
        return sb.ToString();
    }
    
}