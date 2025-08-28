using UnityEngine;

[CreateAssetMenu(menuName = "Laundry Rules/RarityCoinsDebuffedRule")]
public class RarityCoinsDebuffedRule : LaundryRule
{
    public GimmickData.GimmickRarity rarity;

    public override void OnRoundStart(CoinManager manager)
    {
        foreach (var coin in manager.activeCoins)
        {
            if (coin.gimmick.rarity == rarity)
            {
                coin.debuffed = true;
            }    
        }
    }

    public override void OnRoundEnd(CoinManager manager)
    {
        foreach (var coin in manager.activeCoins)
        {
            coin.debuffed = true;
        }
    }
}
