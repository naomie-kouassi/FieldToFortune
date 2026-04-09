using System.Text.Json.Serialization;

namespace FieldToFortune.Model;

public class Player
{
    public string Name { get; set; } = "";
    public double Cash { get; set; } 
    public Portfolio Portfolio { get; set; } = new();
    public List<Call> Calls { get; set; } = [];
    public bool HasExercisableCalls { get; set; }

    public PlayerStats Stats { get; set; } = new();

    [JsonConstructor]
    public Player() {}

    public Player(string name)
    {
        Name = name;
        Cash = 1000;
    }
    

    public void AddCall(Call call) => Calls.Add(call);
    public void RemoveCall(Call call) => Calls.Remove(call);

    public void UpdateCalls()
    {
        HasExercisableCalls = false;
        foreach (var call in Calls.ToList())
        {
            call.Expiry--;
            if (call.IsAtExerciceDate && call.IsProfitable) HasExercisableCalls = true;
            if(call.Expiry<0) RemoveCall(call);
        }
    }
    

}