using UnityEngine;
using static CoinInstance;

[CreateAssetMenu(fileName = "MultiplierData", menuName = "Coins/Multiplier")]
public class MultiplierData : ScriptableObject
{
    public string multiplierName;
    public string multiplierDescription;
    public float headsMultiplier = 1f;
    public float tailsMultiplier = 1f;

    public float ApplyHeads(float baseValue) => baseValue * headsMultiplier;
    public float ApplyTails(float baseValue) => baseValue * tailsMultiplier;

    public float GetMultiplier(bool isHeads)
    {
        return isHeads ? headsMultiplier : tailsMultiplier;
    }
}
