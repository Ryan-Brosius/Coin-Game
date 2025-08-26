using UnityEngine;

[CreateAssetMenu(fileName = "CoinData", menuName = "Coins/Gimmick")]
public class GimmickData : ScriptableObject
{
    public string gimmickName;
    public string gimmickDescription;
    public GimmickEffect[] effects;

    // Adjust odds if we want :)
    public float AdjustHeadsChance(float baseChance)
    {
        float modified = baseChance;
        foreach (var effect in effects)
            modified = effect.AdjustHeadsChance(modified);
        return modified;
    }

    // Callback when coin is flipped if we need to do something
    public float ApplyEffect(float value, bool isHeads, CoinManager manager)
    {
        float modified = value;
        foreach (var effect in effects)
            modified = effect.ApplyEffect(modified, isHeads, manager);
        return modified;
    }

    public void RegisterEvents(CoinManager manager)
    {
        manager.OnCoinEvent += (type, coin, value) =>
        {
            foreach (var effect in effects)
                effect.OnCoinEvent(type, coin, value, manager);
        };
    }
}
