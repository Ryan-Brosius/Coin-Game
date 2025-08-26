using UnityEngine;

[CreateAssetMenu(menuName = "Coins/GimmickEffects/Heavy")]
public class HeavyCoinEffect : GimmickEffect
{
    public override void OnCoinEvent(CoinEventType type, CoinData coin, float value, CoinManager manager)
    {
        if (type == CoinEventType.OnFlipEnd && coin.gimmick == this)
        {
            manager.ReflipAllExceptCurrent(type, coin, value);
        }
    }
}
