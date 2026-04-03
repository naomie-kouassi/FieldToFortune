using ApexCharts;
using FieldToFortune.Model;
using FieldToFortune.Service;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FieldToFortune.Web;
using System.Globalization;

var culture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddApexCharts();



List<Commodity> commodities =
[
    new ("Wheat", 95, 0.03),
    new ("Corn", 150, 0.05),
    new ("Orange Juice", 180, 0.08),
    new ("Coffee", 200, 0.1),
    new ("Cocoa", 320, 0.12)
];
Market market = new Market(commodities);

var gameState = new GameState(market);
var tradingService = new TradingService();


// ── Register them in the DI container ─────────────────────
// AddSingleton = one single instance shared across all pages

builder.Services.AddSingleton(gameState);
builder.Services.AddSingleton(tradingService);

await builder.Build().RunAsync();