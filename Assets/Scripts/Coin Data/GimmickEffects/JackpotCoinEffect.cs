using UnityEngine;

[CreateAssetMenu(menuName = "Coins/GimmickEffects/Jackpot Coin")]
public class JackpotCoinEffect : GimmickEffect
{
    public override float ApplyEffect(float value, bool isHeads, CoinManager manager, CoinInstance coinInstance)
    {
        if (isHeads)
        {
            manager.Jackpot(coinInstance);
        }

        return value;
    }
}
