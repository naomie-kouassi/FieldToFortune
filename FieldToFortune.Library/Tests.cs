using FieldToFortune.Model;

public class Tests
{
    static Commodity wheat = new Commodity("Wheat", 150, 0.15);
    static Commodity coffee = new Commodity("Coffee", 300, 0.3);
    
    public static void TestCommodity()
    {
       
        Console.WriteLine(wheat);
        wheat.SetPrice(300);
        Console.Write(wheat);
    }

    public static void TestPortfolio()
    {
        Portfolio p = new Portfolio();
        p.AddCommodity(wheat,0);
        p.AddCommodity(coffee,5);
        Console.WriteLine(p);
        p.AddCommodity(wheat,2);
        p.RemoveCommodity(coffee,3);
        Console.WriteLine("\n"+p);
        
    }

    public static void TestTransaction()
    {
        //Transaction t1 = new Transaction(TransactionType.Purchase, wheat, 2,300);
        //Transaction t2 = new Transaction(TransactionType.Sale, coffee, 5,500);
        //Console.WriteLine(t1);
        //Console.WriteLine(t2);
    }

    public static void TestSimulatedPrice()
    {
        Market market = new Market([wheat,coffee]);
        var pp = new SimulatedPriceProvider(market);
        for (int i = 1; i < 22; i++)
        {
            Console.WriteLine($"{pp.GetPrice(wheat,i):F2}");
        }
    }

    public static void TestCall()
    {
        var expiry = 3;
        var call = new Call(wheat, 155, expiry);
        Console.WriteLine(call);
        //Console.WriteLine($"Call Price : {call.ComputePrice():F2}$");
        for (var i=expiry; i>0;i--) call.UpdateCall();
        Console.WriteLine(call);
        Console.WriteLine(call.IsAtExerciceDate);
    }
}