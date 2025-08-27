using UnityEngine;

[CreateAssetMenu(menuName = "Coins/GimmickEffects/Heavy")]
public class HeavyCoinEffect : GimmickEffect
{
    public override void OnCoinEvent(CoinEventType type, CoinInstance coin, float value, CoinManager manager)
    {
        if (!manager.OnCoinEventCalledFromCurrentFlippingCoin(coin))
            return;

        if (type == CoinEventType.OnFlipEnd && coin.gimmick.effects.Contains(this))
        {
            manager.ReflipAllBeforeCurrent(type, coin, value);
        }
    }
}
