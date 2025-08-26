using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    // TODO: There should be some sort of like pointer pointing to points and the coin it came from :/
    // The reasoning for this is it could be helpful for certain aspects for callback, like when things get re-flipped
    // but an too lazy at the moment and dont need to worry about it

    [Header("Starting Coins (Templates)")]
    public List<CoinData> startingCoins;
    private List<CoinInstance> activeCoins = new List<CoinInstance>();

    [Header("Coin Gimmicks")]
    public List<GimmickData> gimmicks = new List<GimmickData>();

    public CoinInstance singleCoinDebug;

    //public Dictionary<CoinData, float> pointsMap = new Dictionary<CoinData, float>();

    private void Start()
    {
        foreach (var coinTemplate in startingCoins)
        {
            activeCoins.Add(new CoinInstance(coinTemplate));
        }
    }

    // Event delegates
    public event Action<CoinEventType, CoinData, float> OnCoinEvent;

    public void FlipAllCoins(CoinInstance singleCoin = null)
    {
        float totalPoints = 0;
        List<CoinInstance> flippingCoins;

        // TO DO: REMOVE THIS PIECE OF SHIT LATER
        if (singleCoin != null)
        {
            flippingCoins = new List<CoinInstance> { singleCoin };
        }
        else
        {
            flippingCoins = activeCoins;
        }

        foreach (var coin in flippingCoins)
        {
            float result = FlipCoin(coin);
            if (coin.gimmick)
            {
                Debug.Log($"{coin.coinName} with gimmick <color=green>{coin.gimmick.name}</color> flipped {coin.lastFlippedState} gave {result} points!");
            }
            else
            {
                Debug.Log($"{coin.coinName} flipped {coin.lastFlippedState.ToString()} gave {result} points!");
            }
            totalPoints += result;
        }

        OnCoinEvent?.Invoke(CoinEventType.OnAllCoinsFlipped, null, 0);

        Debug.Log($"Total points: {totalPoints}");
    }

    public float FlipCoin(CoinInstance coin)
    {
        // Step 1: Odds
        float headsChance = coin.gimmick ? coin.gimmick.AdjustHeadsChance(coin.headsChance) : coin.headsChance;
        bool isHeads = UnityEngine.Random.value < headsChance;
        coin.SetFlippedState(isHeads ? CoinInstance.flippedState.Heads : CoinInstance.flippedState.Tails);

        // Step 2: Base Value
        float value = isHeads ? coin.headsValue : coin.tailsValue;

        // Step 3: Apply Multiplier
        if (coin.multiplier)
            value = isHeads ? coin.multiplier.ApplyHeads(value) : coin.multiplier.ApplyTails(value);

        // Step 4: Apply Gimmick effect
        if (coin.gimmick)
            value = coin.gimmick.ApplyEffect(value, isHeads, this);

        return value;
    }

    public void ReflipAllExceptCurrent(CoinEventType type, CoinData coin, float value)
    {
        Debug.Log("Reflipping all other coins...");
    }

    public void FlipSingleCoin(CoinData coin, MultiplierData multiplierData, GimmickData gimmickData)
    {
        singleCoinDebug = new CoinInstance(coin);
        singleCoinDebug.multiplier = multiplierData;
        singleCoinDebug.gimmick = gimmickData;

        FlipAllCoins(singleCoinDebug);
    }
}
