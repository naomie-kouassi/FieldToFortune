using ApexCharts;
using FieldToFortune.Model;
using FieldToFortune.Service;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FieldToFortune.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddApexCharts();


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
var gameState = new GameState(market,player,2000,priceProvider);
var tradingService = new TradingService();


// ── Register them in the DI container ─────────────────────
// AddSingleton = one single instance shared across all pages

builder.Services.AddSingleton(market);
builder.Services.AddSingleton(player);
builder.Services.AddSingleton(priceProvider);
builder.Services.AddSingleton(gameState);
builder.Services.AddSingleton(tradingService);

await builder.Build().RunAsync();