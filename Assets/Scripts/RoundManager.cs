using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }

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
    [SerializeField] GameObject[] coins = new GameObject[10];
    [SerializeField] float startTossDelay = 0.25f;
    [SerializeField] int currentToss = 0;
    [SerializeField] int maxTosses = 10;

    private void Start()
    {
        RoundStart();
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
        foreach(GameObject coin in coins)
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
        foreach (GameObject coin in coins)
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
}
