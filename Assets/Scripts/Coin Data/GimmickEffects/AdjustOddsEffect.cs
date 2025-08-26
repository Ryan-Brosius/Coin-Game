using UnityEngine;

[CreateAssetMenu(menuName = "Coins/GimmickEffects/Adjust Odds")]
public class AdjustOddsEffect : GimmickEffect
{
    [Range(0f, 1f)] public float fixedHeadsChance = 0.5f;

    public override float AdjustHeadsChance(float baseChance)
    {
        return fixedHeadsChance;
    }
}