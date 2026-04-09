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

    public bool HasStarted { get; set; }
    public bool FirstGame { get; set; } = true;
    
    [JsonIgnore] //skipped by the serializer when loading the game
    public IPriceProvider PriceProvider { get; set; }

    public Dictionary<string, double[]> RawPrices { get; set; } = new (); //used for saving prices with JSON
    
    [JsonConstructor]
    public GameState()
    {
        Market = new Market();
        Market.InitializeMarket();
        PriceProvider = new SimulatedPriceProvider(Market,NbTurns);
        RawPrices = new Dictionary<string, double[]>();
    }
    
    public event Action? OnChange;

    public string? NextTurn()
    {
        string? news = null;
        
        if (CurrentTurn < NbTurns) {
            news = Market.RefreshMarket(CurrentTurn, PriceProvider);
            Player.UpdateCalls();
        }
        
        Player.Stats.UpdateNetWorthHistory(Player,Market);
        CurrentTurn++;
        NotifyStateChanged();
        return news;
    }
    
    public void NotifyStateChanged() => OnChange?.Invoke();
    
    public void Reset(Player newPlayer, double winningNetWorth)
    {
        Player = newPlayer;
        WinningNetWorth = winningNetWorth;
        CurrentTurn = 1;
        Market.InitializeMarket();
        // re-run price simulation
        PriceProvider = new SimulatedPriceProvider(Market, NbTurns + 1);
    }
    
    public bool IsDarkMode { get; set; } 
    public event Action? OnThemeChanged;
    public void ToggleTheme()
    {
        IsDarkMode = !IsDarkMode;
        OnThemeChanged?.Invoke();
    }

    public bool HasWon => 
        Player.Stats.GetSnapshot(Player,Market).NetWorth >= WinningNetWorth;

    public bool HasLost=> Player.Stats.GetSnapshot(Player,Market).NetWorth <= LosingNetWorth 
                          || (CurrentTurn>NbTurns && !HasWon);
    
    public bool IsFinished => HasWon || HasLost;
    

}