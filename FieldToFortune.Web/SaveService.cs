using System;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Threading.Tasks;
using FieldToFortune.Model;

namespace FieldToFortune.Service
{
    public class SaveService
    {
        private readonly IJSRuntime _js;
        private const string Key = "ftf-game-session";

        public SaveService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task SaveGame(GameState state)
        {
            // We use options to ensure the JSON is clean
            var options = new JsonSerializerOptions { WriteIndented = false };
            state.RawPrices = state.PriceProvider.GetAllPrices();
            var json = JsonSerializer.Serialize(state, options);
            await _js.InvokeVoidAsync("sessionStorage.setItem", Key, json);
            Console.WriteLine("Saving...");
        }

        public async Task<bool> LoadGame(GameState currentState)
        {
            var json = await _js.InvokeAsync<string>("sessionStorage.getItem", Key);
            
            if (string.IsNullOrEmpty(json)) return false;

            try
            {
                var loadedData = JsonSerializer.Deserialize<GameState>(json);
                if (loadedData != null)
                {
                    // Manually map the "Data" back to your singleton
                    // This preserves your Event Subscriptions (OnThemeChanged)
                    currentState.Player = loadedData.Player;
                    currentState.Market = loadedData.Market;
                    currentState.CurrentTurn = loadedData.CurrentTurn;
                    currentState.IsDarkMode = loadedData.IsDarkMode;
                    currentState.HasStarted = loadedData.HasStarted;
                    var newProvider = new SimulatedPriceProvider(); 
                    newProvider._prices = loadedData.RawPrices;
                    currentState.PriceProvider = newProvider;
                    Console.WriteLine("Data successfully mapped to GameState!");
                    // ... map any other core properties
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Load failed: {ex.Message}");
            }
            return false;
        }

        public async Task ClearSave()
        {
            await _js.InvokeVoidAsync("sessionStorage.removeItem", Key);
        }
    }
}