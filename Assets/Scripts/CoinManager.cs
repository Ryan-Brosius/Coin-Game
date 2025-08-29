using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    // TODO: There should be some sort of like pointer pointing to points and the coin it came from :/
    // The reasoning for this is it could be helpful for certain aspects for callback, like when things get re-flipped
    // but an too lazy at the moment and dont need to worry about it

    public static CoinManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (var coinTemplate in startingCoins)
        {
            var coinInstance = new CoinInstance(coinTemplate);
            activeCoins.Add(coinInstance);

            if (coinInstance.gimmick)
                coinInstance.gimmick.RegisterEvents(this);
        }
    }

    [Header("Starting Coins (Templates)")]
    public List<CoinData> startingCoins;
    public List<CoinInstance> activeCoins = new List<CoinInstance>();
    public List<CoinInstance> flippedOrder = new List<CoinInstance>();

    private int maxFlips = 10;
    public int currentMaxFlips;

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

    [Header("Round Rules")]
    public LaundryRule activeRoundRule;


    ///////////////////////////////////////////////////
    //// THIS IS WHERE THE CODE FOR THE ACTUAL GAME ///
    ///                  STARTS!!!                  ///
    ///////////////////////////////////////////////////


    //TODO MOVE THIS INTO SOMETHING ELSE LATER SO WE HAVE SOME SORT OF FLOW
    private void Start()
    {
        currentMaxFlips = maxFlips;
    }

    public void StartRound()
    {
        flippedOrder.Clear();
        activeRoundRule?.OnRoundStart(this);
    }

    public void EndRound()
    {
        flippedOrder.Clear();
        activeRoundRule?.OnRoundEnd(this);
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
        StartRound();
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
            Debug.Log(coin.FlippedDebugInfo());
        }

        OnCoinEvent?.Invoke(CoinEventType.OnAllCoinsFlipped, null, 0);

        Debug.Log($"Total points: {flippingCoins.Sum(c => c.CoinValue)}");
        EndRound();
    }

    public float FlipCoin(CoinInstance coin)
    {
        CurrentFlippingCoin = coin; // bruh had a bug because I forgot about this :/

        flippedOrder.Add(coin);
        coin.ResetCoinChances();
        activeRoundRule?.OnBeforeCoinFlip(this, coin);   
        coin.PrepareCoinChances(this);
        coin.FlipCoin(this);
        activeRoundRule?.OnAfterCoinFlip(this, coin);


        Debug.Log(coin.FlippedDebugText());
        Debug.Log(coin.FlippedDebugInfo());
        Debug.Log($"Total points: {GetCurrentTotalRoundScore()}");

        return coin.CoinValue;
    }

    public float GetCoinChances(CoinInstance coin)
    {
        coin.PrepareCoinChances(this);
        return coin.currentHeadsChance;
    }

    //TODO: Fix this later to like actually do stuff
    public void ReflipAllBeforeCurrent(CoinEventType type, CoinInstance coin, float value)
    {
        Debug.Log("Reflipping previous coins...");

        void ReflipCoinHelper(CoinInstance coin)
        {
            coin.ResetCoinChances();
            activeRoundRule?.OnBeforeCoinFlip(this, coin);
            coin.PrepareCoinChances(this);
            coin.FlipCoin(this);
            activeRoundRule?.OnAfterCoinFlip(this, coin);
        }

        foreach (var c in flippedOrder)
        {
            if (c == coin)
                return;


            ReflipCoinHelper(c);
            if (c.MyGameobject.TryGetComponent<CoinTossAnimation>(out CoinTossAnimation coinTossAnimation))
                coinTossAnimation.RetossCoin();

            Debug.Log(c.FlippedDebugText());
        }
    }

    //TODO: Fix this later to like actually choose and reflip coin
    public void AddRetoss(CoinInstance coin)
    {
        Debug.Log("Adding a free retoss...");
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

    public float GetCurrentTotalRoundScore()
    {
        float score = 0;

        foreach (var coin in flippedOrder)
        {
            if (!coin.multiplier)
            {
                score += coin.CoinValue;
            }
            else
            {
                score *= coin.multiplier.GetMultiplier(coin.lastFlippedState == CoinInstance.flippedState.Heads);
            }
        }
        float roundedScore = Mathf.Round(score);
        return roundedScore;
    }
}
