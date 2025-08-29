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

    public float currentValue;
    public float CoinValue => currentValue;
    public GameObject CoinPrefab => template.coinPrefab;
    public GameObject MyGameobject;

    public MultiplierData multiplier;
    public GimmickData gimmick;

    public bool debuffed = false;

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

    public void ResetCoinChances()
    {
        currentHeadsChance = 0;
    }

    public void PrepareCoinChances(CoinManager coinManager)
    {
        currentHeadsChance += (gimmick && !debuffed) ? gimmick.AdjustHeadsChance(baseHeadsChance) : baseHeadsChance;
        coinManager.CoinEventBeforeCoinFlip(this);

        // After all modifiers are made keep it between 0 - 1 :')
        currentHeadsChance = Mathf.Clamp01(currentHeadsChance);
    }

    public void FlipCoin(CoinManager coinManager)
    {
        bool isHeads = UnityEngine.Random.value < currentHeadsChance;

        lastFlippedState = isHeads ? CoinInstance.flippedState.Heads : CoinInstance.flippedState.Tails;

        currentValue = isHeads ? baseHeadsValue : baseTailsValue;

        if (multiplier)
            currentValue = isHeads ? multiplier.ApplyHeads(currentValue) : multiplier.ApplyTails(currentValue);

        if (gimmick && !debuffed)
            currentValue = gimmick.ApplyEffect(currentValue, isHeads, coinManager, this);

        // Now exists in the animation
        //coinManager.CoinEventFlipEnd(this);
    }

    public string FlippedDebugText()
    {
        return $"<color=green>{CoinName}</color> flipped {lastFlippedState} gave {CoinValue} points!";
    }

    public string FlippedDebugInfo()
    {
        return
            $"<b><color=yellow>[Coin Debug Info]</color></b>\n" +
            $"Name: <color=green>{CoinName}</color>\n" +
            $"Last Flip: {lastFlippedState}\n" +
            $"Base Heads Value: {baseHeadsValue}\n" +
            $"Base Tails Value: {baseTailsValue}\n" +
            $"Base Heads Chance: {baseHeadsChance * 100f}%\n" +
            $"Current Heads Chance: {currentHeadsChance * 100f}%\n" +
            $"Current Coin Value: {currentValue}\n" +
            $"Multiplier: {(multiplier != null ? multiplier.name : "None")}\n" +
            $"Gimmick: {(gimmick != null ? gimmick.name : "None")}\n";
    }
}
