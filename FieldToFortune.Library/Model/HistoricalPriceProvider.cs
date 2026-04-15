using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace FieldToFortune.Model;

public class HistoricalPriceProvider : IPriceProvider
{
    private readonly HttpClient _http = new();
    private readonly string _apiKey = "3a853dd60fc7bab497f1ee10843e9c34";
    public Dictionary<string, double[]> _cache = new();

    public HistoricalPriceProvider()
    {
        _ = InitializeAll();
    }

    public async Task InitializeAll()
    {
        await Initialize("Corn", "PMAIZMTUSDM");
        await Initialize("Rice", "PRICENPQUSDM");
        await Initialize("Barley", "PBARLUSDM");
        await Initialize("Bananas", "PBANSOPUSDM");
        await Initialize("Sunflower Oil", "PSUNOUSDM");
        await Initialize("Cocoa", "PCOCOUSDM");
    }

    public async Task Initialize(string commodidtyName, string seriesId)
    {
        var url =
            $"https://corsproxy.io/?https://api.stlouisfed.org/fred/series/observations?series_id={seriesId}"+
                $"&api_key={_apiKey}&frequency=m&sort_order=desc&file_type=json&limit={GameState.NbTurns}";
        
        var response = await _http.GetFromJsonAsync<FredResponse>(url);
        
        if (response?.Observations == null) {
            _cache[commodidtyName]=[];
            return;
        }

        _cache[commodidtyName] = response.Observations
            .Select(obs => double.TryParse(obs.Value, out double val) ? val : 0.0)
            .Reverse() 
            .ToArray();

    }

    public double GetPrice(string commodityName, int turn) 
    {
        return _cache[commodityName][turn-1];
    }
    
    public double[] GetFirstPrices(string commodityName, int lastTurn)
    {
        return _cache[commodityName][..lastTurn];
    }

    public Dictionary<string, double[]> GetAllPrices()
    {
        return _cache;
    }

    public Dictionary<string, double> GetAllPrices(int turn)
    {
        var allPrices = new Dictionary<string, double>();
        foreach (var commodityName in _cache.Keys) allPrices[commodityName] = _cache[commodityName][turn];
        return allPrices;
    }
    
    public class FredResponse
    {
        public List<Observation> Observations { get; set; } = [];
    }

    public class Observation
    { 
        [JsonPropertyName("value")]
        public string Value { get; set; } = string.Empty;
    }
    
    
}