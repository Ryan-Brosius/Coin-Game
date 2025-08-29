using UnityEngine;

[CreateAssetMenu(menuName = "Coins/GimmickEffects/Arcade Coin")]
public class ArcadeCoinEffect : GimmickEffect
{
    public override float ApplyEffect(float value, bool isHeads, CoinManager manager, CoinInstance coinInstance)
    {
        if (isHeads)
        {
            manager.AddRetoss(coinInstance);
        }

        return value;
    }
}
