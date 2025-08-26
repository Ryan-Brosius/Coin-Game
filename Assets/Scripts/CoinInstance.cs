using UnityEngine;

public class CoinInstance
{
    public CoinData template; // Reference to the coin data SO
    public float headsValue;
    public float tailsValue;
    public float headsChance;

    public MultiplierData multiplier;
    public GimmickData gimmick;

    public string coinName => template.coinName;

    public enum flippedState
    {
        Heads,
        Tails,
        Side
    }
    [HideInInspector] public flippedState lastFlippedState;

    public CoinInstance(CoinData template)
    {
        this.template = template;
        this.headsValue = template.headsValue;
        this.tailsValue = template.tailsValue;
        this.headsChance = template.headsChance;

        multiplier = template.multiplier;
        gimmick = template.gimmick;
    }

    public void SetFlippedState(flippedState flippedState)
    {
        lastFlippedState = flippedState;
    }
}
