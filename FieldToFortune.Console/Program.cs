using FieldToFortune.Model;
using FieldToFortune.UI;

Market market = new Market();
market.AddCommodity(new Commodity("Wheat", 95, 0.03));
market.AddCommodity(new Commodity("Corn", 150, 0.05));
market.AddCommodity(new Commodity("Orange Juice", 180, 0.08));
market.AddCommodity(new Commodity("Coffee", 200, 0.1));
market.AddCommodity(new Commodity("Cacao", 320, 0.12));

var player = new Player("Naomie");

foreach (Commodity commodity in market.Commodities)
{
    player.Portfolio.AddCommodity(commodity, 0);
}


var priceProvider = new SimulatedPriceProvider(market.Commodities);
var ui = new ConsoleUI(player, market, priceProvider);
ui.Run();