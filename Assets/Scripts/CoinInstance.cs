using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CoinInstance
{
    // Had to create this as a wrapper for the CoinData class
    // This should handle and logic that the coins need to have
    // Coins kinda are set up like a state machine tbh and thats okay!

    public CoinData template; // Reference to the coin data SO
    public float baseHeadsValue;
    public float baseTailsValue;
    public float baseHeadsChance;

    private float currentHeadsValue;
    private float currentTailsValue;
    public float currentHeadsChance;

    private float currentValue;
    public float CoinValue => currentValue;

    public MultiplierData multiplier;
    public GimmickData gimmick;

    public string CoinName
    {
        get
        {
            if (gimmick)
                return $"{gimmick.gimmickName} {template.coinName}";
            return template.name;
        }
    }

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
        this.baseHeadsValue = template.headsValue;
        this.baseTailsValue = template.tailsValue;
        this.baseHeadsChance = template.headsChance;
        
        if (template.multiplier)
            multiplier = ScriptableObject.Instantiate(template.multiplier);
        else
            multiplier = null;

        if (template.gimmick)
        {
            gimmick = ScriptableObject.Instantiate(template.gimmick);

            var newEffects = new List<GimmickEffect>();

            foreach (var effect in template.gimmick.effects)
            {
                if (effect != null)
                {
                    var clonedEffect = ScriptableObject.Instantiate(effect);
                    newEffects.Add(clonedEffect);
                }
            }

            gimmick.effects = newEffects;
        }
        else
            gimmick = null;
    }

    public void FlipCoin(CoinManager coinManager)
    {
        currentHeadsChance = gimmick ? gimmick.AdjustHeadsChance(baseHeadsChance) : baseHeadsChance;
        coinManager.CoinEventBeforeCoinFlip(this);
        bool isHeads = UnityEngine.Random.value < currentHeadsChance;

        lastFlippedState = isHeads ? CoinInstance.flippedState.Heads : CoinInstance.flippedState.Tails;

        currentValue = isHeads ? baseHeadsValue : baseTailsValue;

        if (multiplier)
            currentValue = isHeads ? multiplier.ApplyHeads(currentValue) : multiplier.ApplyTails(currentValue);

        if (gimmick)
            currentValue = gimmick.ApplyEffect(currentValue, isHeads, coinManager, this);

        coinManager.CoinEventFlipEnd(this);
    }

    public string FlippedDebugText()
    {
        return $"<color=green>{CoinName}</color> flipped {lastFlippedState} gave {CoinValue} points!";
    }
}
