using System.Text.Json.Serialization;

namespace FieldToFortune.Model;

public class GameState
{
    public Market Market { get; set; }
    public Player Player { get; set; } = new ("");
    public double WinningNetWorth { get; set; } = 1500;
    private const double LosingNetWorth = 500;

    public int CurrentTurn { get; set; } = 1;
    public const int NbTurns = 20;

    public bool HasStarted { get; set; }= false;
    public bool InConfiguration = true;

    [JsonIgnore] //skipped by the serialazer when loading the game
    public IPriceProvider PriceProvider { get; set; }
    
    public Dictionary<string, double[]> RawPrices { get; set; }
    
    [JsonConstructor]
    public GameState()
    {
        Market = new Market([]);
        PriceProvider = new SimulatedPriceProvider();
        RawPrices = new Dictionary<string, double[]>();
    }

    public GameState(Market market)
    {
        Market = market;
        PriceProvider = new SimulatedPriceProvider(market);
        RawPrices = new Dictionary<string, double[]>();
    }
    
    public GameState(Market market, Player player, double winningNetWorth, IPriceProvider priceProvider)
    {
        Market = market;
        Player = player;
        WinningNetWorth = winningNetWorth;
        PriceProvider = priceProvider;
    }
    
    public event Action? OnChange;
    
    public string? NextTurn() { 
        string? news = Market.RefreshMarket(CurrentTurn,PriceProvider);

        Player.UpdateCalls();
        Player.UpdateNetWorthHistory(Market);
        
        CurrentTurn++;
        NotifyStateChanged();
        return news;
    }
    
    public void NotifyStateChanged() => OnChange?.Invoke();
    
    public void Reset(Player player, double winningNetWorth)
    {
        Player = player;
        WinningNetWorth = winningNetWorth;
        CurrentTurn = 1;
        // re-run price simulation
        PriceProvider = new SimulatedPriceProvider(Market);
    }
    
    public bool IsDarkMode { get; set; } = false;
    public event Action? OnThemeChanged;
    public void ToggleTheme()
    {
        IsDarkMode = !IsDarkMode;
        OnThemeChanged?.Invoke();
    }

    public bool HasWon => Player.NetWorth(Market) >= WinningNetWorth;

    public bool HasLost=> Player.NetWorth(Market) < LosingNetWorth || 
               (CurrentTurn>=NbTurns+1 && !HasWon);
    
    public bool IsFinished => HasWon || HasLost;
    

}