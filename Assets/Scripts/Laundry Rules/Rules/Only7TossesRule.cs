using UnityEngine;

[CreateAssetMenu(menuName = "Laundry Rules/Only7TossesRule")]
public class Only7TossesRule : LaundryRule
{
    public override void OnRoundStart(CoinManager manager)
    {
        manager.currentMaxFlips = 7;
    }
}
