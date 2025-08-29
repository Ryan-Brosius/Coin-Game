using UnityEngine;

[CreateAssetMenu(menuName = "Coins/GimmickEffects/Lucky Coin")]
public class LuckyCoinEffect : GimmickEffect
{
    [Range(0f, 1f)] public float edgeChance;
    public float multiplier;

    public override float ApplyEffect(float value, bool isHeads, CoinManager manager, CoinInstance coinInstance)
    {
        if (UnityEngine.Random.value < edgeChance)
        {
            coinInstance.lastFlippedState = CoinInstance.flippedState.Side;
            return coinInstance.baseHeadsValue * multiplier;
        }

        return value;
    }
}