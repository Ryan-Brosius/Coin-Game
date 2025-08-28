using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }
    private CoinManager coinManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] List<Transform> coinTargets = new List<Transform>(10);
    [SerializeField] GameObject[] coinsPos = new GameObject[10];
    private List<GameObject> Coins = new List<GameObject>();
    [SerializeField] float startTossDelay = 0.25f;
    [SerializeField] int currentToss = 0;
    [SerializeField] int maxTosses = 10;

    private void Start()
    {
        // Connect the Coin Manager to the round manager
        coinManager = CoinManager.Instance;
        // activeCoins

        SpawnCoins();
        RoundStart();
    }

    private void SpawnCoins()
    {
        foreach (var coin in coinsPos)
        {
            coin.SetActive(false);
        }

        var CoinsRefs = coinManager.activeCoins.Select(c => c.CoinPrefab).ToArray();

        for (int i = 0; i < CoinsRefs.Count(); i++)
        {
            var coinGameobject = GameObject.Instantiate(CoinsRefs[i], coinsPos[i].transform.position, Quaternion.identity);
            Coins.Add(coinGameobject);
            coinManager.activeCoins[i].MyGameobject = coinGameobject;
        }
    }

    public void RoundStart()
    {
        currentToss = 0;
        StartCoroutine(StartAnimation());
    }

    public void EndRound()
    {
        currentToss = 0;
        StartCoroutine(ReturnAnimation());

    }

    public IEnumerator StartAnimation()
    {
        foreach(GameObject coin in Coins)
        {
            if (coin.TryGetComponent<CoinTossAnimation>(out CoinTossAnimation coinAnim))
            {
                coinAnim.CoinStartAnimation();
                yield return new WaitForSeconds(startTossDelay);
            }
        }
        yield return null;
    }

    public IEnumerator ReturnAnimation()
    {
        foreach (GameObject coin in Coins)
        {
            if (coin.TryGetComponent<CoinTossAnimation>(out CoinTossAnimation coinAnim))
            {
                coinAnim.ReturnCoin();
                yield return new WaitForSeconds(startTossDelay);
            }
        }
        yield return null;
    }

    public Transform GetCoinTarget()
    {
        currentToss++;
        if ((currentToss - 1) < coinTargets.Count)
        {
            return coinTargets[currentToss - 1];
        }
        else
        {
            return coinTargets[0];
        }
    }

    public void FlipCoinUsingGameobjectReference(GameObject coin)
    {
        coinManager.FlipCoin(GetCoinInstanceFromGameObject(coin));
    }

    public CoinInstance GetCoinInstanceFromGameObject(GameObject coin)
    {
        var index = Coins.IndexOf(coin);
        return coinManager.activeCoins[index];
    }
}
