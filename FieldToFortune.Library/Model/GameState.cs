namespace FieldToFortune.Model;

public class GameState
{
    public Market Market { get; private set; }
    public Player Player { get; private set; } = new ("");
    public double WinningNetWorth { get; private set; } = 1500;
    private const double LosingNetWorth = 500;

    public int CurrentTurn { get; private set; } = 1;
    public const int NbTurns = 20;

    public bool HasStarted = false;
    public bool InConfiguration = true;

    public IPriceProvider PriceProvider { get; private set; }

    public GameState(Market market)
    {
        Market = market;
        PriceProvider = new SimulatedPriceProvider(market);
    }
    
    public GameState(Market market, Player player, double winningNetWorth, IPriceProvider priceProvider)
    {
        Market = market;
        Player = player;
        WinningNetWorth = winningNetWorth;
        PriceProvider = priceProvider;
    }
    
    public event Action? OnChange;
    
    public MarketNews? NextTurn() { 
        MarketNews? news = Market.RefreshMarket(CurrentTurn,PriceProvider);

        foreach (Call call in Player.Calls.Keys)
        {
            call.UpdateCall();
            if(call.Expiry<0) Player.RemoveCall(call);
        }
        
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
    
    public bool IsDarkMode { get; private set; } = false;
    public event Action? OnThemeChanged;
    public void ToggleTheme()
    {
        IsDarkMode = !IsDarkMode;
        OnThemeChanged?.Invoke();
    }

    public bool HasWon => Player.NetWorth >= WinningNetWorth;

    public bool HasLost=> Player.NetWorth < LosingNetWorth || 
               (CurrentTurn>=NbTurns+1 && !HasWon);
    
    public bool IsFinished => HasWon || HasLost;
    

}