using UnityEngine;

[CreateAssetMenu(menuName = "Coins/GimmickEffects/Vintage Coin")]
public class VintageCoinEffect : GimmickEffect
{
    public float additionalCoinValue;

    public override float ApplyEffect(float value, bool isHeads, CoinManager manager, CoinInstance coinInstance)
    {
        return value + additionalCoinValue;
    }
}
