using System.Text;

namespace FieldToFortune.Model;

public enum TransactionType
{
    Purchase, Sale, CallPremium, CallExercise
}

public record Transaction(TransactionType Type, string Description, double Amount, int Turn)
{
    
    public override string ToString()
    {
        return Description+ $"for {Math.Abs(Amount):F2}$";
    }
};


