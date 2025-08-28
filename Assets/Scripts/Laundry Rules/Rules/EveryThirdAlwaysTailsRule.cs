using UnityEngine;

[CreateAssetMenu(menuName = "Laundry Rules/EveryThirdAlwaysTails")]
public class EveryThirdAlwaysTailsRule : LaundryRule
{
    private int flipCount;

    public override void OnRoundStart(CoinManager manager)
    {
        flipCount = 0;
    }

    public override void OnBeforeCoinFlip(CoinManager manager, CoinInstance coin)
    {
        flipCount++;

        if (flipCount % 3 == 0)
        {
            // Pretty much guarentees it will be tails :')
            // but idk may be finicky so might need to fix :/
            coin.currentHeadsChance = -10;
        }
    }
}
