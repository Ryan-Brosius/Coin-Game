using UnityEngine;

[CreateAssetMenu(menuName = "Laundry Rules/EveryThirdAlwaysNegativeRule")]
public class EveryThirdAlwaysNegativeRule : LaundryRule
{
    private int flipCount;

    public override void OnRoundStart(CoinManager manager)
    {
        flipCount = 0;
    }

    public override void OnAfterCoinFlip(CoinManager manager, CoinInstance coin)
    {
        flipCount++;

        if (flipCount % 3 == 0)
        {
            if (coin.CoinValue > 0)
            {
                coin.currentValue *= -1;
            }
        }
    }
}
