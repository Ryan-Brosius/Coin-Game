using UnityEngine;

[CreateAssetMenu(menuName = "Coins/GimmickEffects/Adjust Mult")]
public class AdjustHeadsMultEffect : GimmickEffect
{
    public float headsMult = 1f;
    public float tailsMult = 1f;

    public override float ApplyEffect(float value, bool isHeads, CoinManager manager, CoinInstance coinInstance)
    {
        return isHeads ? value * headsMult : value * tailsMult;
    }
}
