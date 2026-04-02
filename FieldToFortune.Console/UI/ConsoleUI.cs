using FieldToFortune.Model;
using FieldToFortune.Service;

namespace FieldToFortune.UI;

public class ConsoleUI
{
    private Player player;
    private Market market;
    private TradingService tradingService;
    private GameState _gameState;

    public ConsoleUI(Player player, Market market, IPriceProvider priceProvider)
    {
        this.player = player;
        this.market = market;
        this.tradingService = new TradingService();
        this._gameState = new GameState(market,player, 2000,priceProvider);
    }

    public void Run()
    {
        bool running = true;
        while (running)
        {
            DisplayMainMenu();
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    DisplayMarket();
                    break;
                case 2:
                    DisplayPortfolio();
                    break;
                case 3:
                    DisplayCalls();
                    break;
                case 4:
                    Buy();
                    break;
                case 5:
                    Sell();
                    break;
                case 6:
                    BuyCall();
                    break;
                case 7:
                    DisplayHistory();
                    break;
                case 8:
                    NextTurn();
                    break;
                case 9:
                    running = false;
                    break;
                default :
                    Console.WriteLine("Invalid choice");
                    break;
                
            }
        }
    }
    
    private void DisplayMainMenu()
    {
        Console.WriteLine("\n====== MAIN MENU =====");
        Console.WriteLine("1. View market");
        Console.WriteLine("2. View portfolio");
        Console.WriteLine("3. View call options");
        Console.WriteLine("4. Buy commodities");
        Console.WriteLine("5. Sell commodities");
        Console.WriteLine("6. Buy call options");
        Console.WriteLine("7. View history");
        Console.WriteLine("8. Next turn");
        Console.WriteLine("9. Exit");
    }

    private void DisplayMarket()
    {
        Console.WriteLine("\n====== MARKET =====");
        DisplayCommodities();
    }

    private void DisplayCommodities()
    {
        for (int i = 0; i < market.Commodities.Count; i++)
        {
            Console.WriteLine(i+1+". " + market.Commodities[i]);
        }
    }

    private void DisplayPortfolio()
    {
        Console.WriteLine("\n====== PORTFOLIO =====");
        Console.WriteLine(player.Portfolio);
    }

    private void DisplayCalls()
    {
        Console.WriteLine("\n====== CALLS =====");
        if (player.Calls.Count==0) {Console.WriteLine ("No calls");}
        else
        {
            List<Call> exercisables = [];
            foreach (Call call in player.Calls.Keys)
            {
                Console.WriteLine($"x {player.Calls[call]} " + call);
                if (call.IsAtExerciceDate) exercisables.Add(call);
            }

            foreach (Call call in exercisables)
            {
                Console.WriteLine($" {call} is at exercise date!");
                Console.WriteLine("Exercise this call? (Y/N)");
                var choice = Console.ReadLine();
                while (choice == null || (choice.ToUpper() != "Y" && choice.ToUpper() != "N"))
                {
                    Console.WriteLine("Please enter a valid choice.");
                    choice = Console.ReadLine();
                }

                if (choice.ToUpper() == "Y") tradingService.ExerciseCall(player, call, _gameState.CurrentTurn);
            }
        }
    }
    
    private void Buy()
    {
        Console.WriteLine("\n====== BUY MENU =====");
        DisplayCommodities();
        
        Console.WriteLine("Choose a commodity: ");
        int index = Convert.ToInt32(Console.ReadLine());
        Commodity? commodity = market.GetCommodity(index-1);
        if (commodity == null)
        {
            Console.WriteLine("Invalid action!");
            return;
        }
        
        Console.WriteLine("Quantity: ");
        var quantity = Convert.ToInt32(Console.ReadLine());
        Transaction? t = tradingService.BuyCommodity(player, commodity, quantity, _gameState.CurrentTurn);
        if (t == null)
        {
            Console.WriteLine("Purchase failed.");
            return;
        }
        
        
        Console.WriteLine("Successful purchase.");
        Console.WriteLine($"Current balance: {player.Cash}$");

    }

    private void Sell()
    {
        Console.WriteLine("\n ===== SELL MENU =====");
        Console.Write('\t');
        DisplayPortfolio();
        if (player.Portfolio.IsEmpty())
        {
            Console.WriteLine("No commodities to sell!");
            return;
        }
        Console.WriteLine("Choose a commodity: ");
        int index = Convert.ToInt32(Console.ReadLine());
        Commodity? commodity = market.GetCommodity(index-1);
        if (commodity == null)
        {
            Console.WriteLine("Invalid action!");
            return;
        }
        Console.WriteLine("Quantity: ");
        var quantity = Convert.ToInt32(Console.ReadLine());
        Transaction? t = tradingService.SellCommodity(player, commodity, quantity, _gameState.CurrentTurn);
        
        if (t == null)
        {
            Console.Write("Sale failed.");
            return;
        }
        
        
        Console.WriteLine("Successful sale.");
        Console.WriteLine($"Current balance: {player.Cash}$");

    }

    private void BuyCall()
    {
        Console.WriteLine("\n====== BUY CALL MENU =====");
        DisplayCommodities();
        
        Console.WriteLine("Choose an underlying commodity: ");
        int index = Convert.ToInt32(Console.ReadLine());
        Commodity? commodity = market.GetCommodity(index-1);
        if (commodity == null)
        {
            Console.WriteLine("Invalid action!");
            return;
        }
        
        Console.WriteLine("Quantity: ");
        var quantity = Convert.ToInt32(Console.ReadLine());
        
        Console.WriteLine("Strike price: ");
        var strikePrice = Convert.ToDouble(Console.ReadLine());
        
        Console.WriteLine("Exercise: ");
        var expiry = Convert.ToInt32(Console.ReadLine());
        
        Transaction? t = tradingService.BuyCall(player, commodity, strikePrice, expiry, quantity, _gameState.CurrentTurn);
        if (t == null)
        {
            Console.WriteLine("Purchase failed.");
            return;
        }
        
        
        Console.WriteLine("Successful purchase.");
        Console.WriteLine($"Current balance: {player.Cash}$");
    }

    private void DisplayHistory()
    {
        Console.WriteLine("\n ===== HISTORY =====");
        foreach (var transaction in player.TransactionHistory )
        {
            Console.WriteLine(transaction);
        }
    }

    private void NextTurn()
    {
        _gameState.NextTurn();
    }
    
}