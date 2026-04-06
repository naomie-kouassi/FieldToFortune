using System;
using System.Collections.Generic;
using ApexCharts;
using FieldToFortune.Model;
using FieldToFortune.Service;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FieldToFortune.Web;
using System.Globalization;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;


// ---- Ensure app uses the dot comma for doubles (ie 1.2 and not 1,2) ----
var culture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;


// ---- Initialize the web app ----
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped<SaveService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddApexCharts();


// ---- Initialize game variables ----
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


//  ---- Share created GameState and TradingService with all app pages ----
builder.Services.AddSingleton(gameState);
builder.Services.AddSingleton(tradingService);


// Launch app
await builder.Build().RunAsync();