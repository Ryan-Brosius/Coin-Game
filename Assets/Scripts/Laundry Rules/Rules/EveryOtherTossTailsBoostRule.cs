using UnityEngine;


[CreateAssetMenu(menuName = "Laundry Rules/EveryOtherTossTailsBoostRule")]
public class EveryOtherTossTailsBoostRule : LaundryRule
{
    [Range(0f, 1f)] public float tailsBoostChance;
    private int flipCount;

    public override void OnRoundStart(CoinManager manager)
    {
        flipCount = 0;
    }

    public override void OnBeforeCoinFlip(CoinManager manager, CoinInstance coin)
    {
        if (flipCount % 2 == 1)
        {
            coin.currentHeadsChance -= tailsBoostChance;
        }

        flipCount++;
    }
}
