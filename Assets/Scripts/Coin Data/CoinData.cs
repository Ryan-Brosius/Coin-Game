using UnityEngine;

[CreateAssetMenu(fileName = "CoinData", menuName = "Coins/Base Coin")]
public class CoinData : ScriptableObject
{
    public string coinName;
    public string coinDescription;
    public float headsValue;
    public float tailsValue;

    public GameObject coinPrefab;

    [Range(0f, 1f)] public float headsChance = 0.5f; // 50/50 default :p

    // If these are null then they just dont exist for this LOL!!!
    public MultiplierData multiplier;
    // Maybe in the future this can be a list of gimmicks(?)
    // for right now just make it a single gimmick
    public GimmickData gimmick;
}