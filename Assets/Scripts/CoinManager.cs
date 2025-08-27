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

    public CoinInstance CurrentFlippingCoin;
    public CoinInstance NextFlippingCoin
    {
        get
        {
            if (CurrentFlippingCoin == null) return null;

            int index = activeCoins.IndexOf(CurrentFlippingCoin);
            if (index >= 0 && index < activeCoins.Count - 1)
            {
                return activeCoins[index + 1];
            }

            return null;
        }
    }
    public CoinInstance NextCoinAfterThis(CoinInstance coin)
    {
        int index = activeCoins.IndexOf(coin);
        if (index >= 0 && index < activeCoins.Count - 1)
        {
            return activeCoins[index + 1];
        }

        return null;
    }
    public bool OnCoinEventCalledFromCurrentFlippingCoin(CoinInstance coin)
    {
        return CurrentFlippingCoin == coin;
    }

    [Header("Coin Gimmicks")]
    public List<GimmickData> gimmicks = new List<GimmickData>();

    public CoinInstance singleCoinDebug;

    //public Dictionary<CoinData, float> pointsMap = new Dictionary<CoinData, float>();

    private void Start()
    {
        foreach (var coinTemplate in startingCoins)
        {
            var coinInstance = new CoinInstance(coinTemplate);
            activeCoins.Add(coinInstance);
            coinInstance.gimmick.RegisterEvents(this);
        }
    }

    // Event delegates
    public event Action<CoinEventType, CoinInstance, float> OnCoinEvent;
    public void CoinEventBeforeCoinFlip(CoinInstance coinInstance)
    {
        OnCoinEvent?.Invoke(CoinEventType.BeforeCoinFlip, coinInstance, coinInstance.CoinValue);
    }
    public void CoinEventFlipEnd(CoinInstance coinInstance)
    {
        OnCoinEvent?.Invoke(CoinEventType.OnFlipEnd, coinInstance, coinInstance.CoinValue);
    }

    public void FlipAllCoinsDebug()
    {
        FlipAllCoins();
    }

    public void FlipAllCoins(CoinInstance singleCoin = null)
    {
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
            CurrentFlippingCoin = coin;

            FlipCoin(coin);

            Debug.Log(coin.FlippedDebugText());
        }

        OnCoinEvent?.Invoke(CoinEventType.OnAllCoinsFlipped, null, 0);

        Debug.Log($"Total points: {flippingCoins.Sum(c => c.CoinValue)}");
    }

    public float FlipCoin(CoinInstance coin)
    {
        // Flipping the coin
        coin.FlipCoin(this);

        return coin.CoinValue;
    }

    //TODO: Fix this later to like actually do stuff
    public void ReflipAllBeforeCurrent(CoinEventType type, CoinInstance coin, float value)
    {
        Debug.Log("Reflipping previous coins...");

        foreach (var c in activeCoins)
        {
            if (c == coin)
                return;

            FlipCoin(c);
            Debug.Log(c.FlippedDebugText());
        }
    }

    //TODO: Fix this later to like actually choose and reflip coin
    public void RetossAlreadyFlippedCoin(CoinInstance coin)
    {
        Debug.Log("Reflipping one of your coins...");
    }

    //TODO: Fix this later for jackpot!!
    public void Jackpot(CoinInstance coin)
    {
        Debug.Log("Jackpot Hit...");
    }

    public void FlipSingleCoin(CoinData coin, MultiplierData multiplierData, GimmickData gimmickData)
    {
        singleCoinDebug = new CoinInstance(coin);
        singleCoinDebug.multiplier = multiplierData;
        singleCoinDebug.gimmick = gimmickData;

        coin.gimmick.RegisterEvents(this);

        FlipAllCoins(singleCoinDebug);
    }
}
