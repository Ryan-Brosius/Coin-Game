using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CoinData", menuName = "Coins/Gimmick")]
public class GimmickData : ScriptableObject
{
    public string gimmickName;
    public string gimmickDescription;
    public List<GimmickEffect> effects = new List<GimmickEffect>();
    public GimmickRarity rarity;

    public enum GimmickRarity
    {
        Common,
        Uncommon,
        Rare,
    }

    // what the fuck was I cooking when I made this shit?
    public float AdjustHeadsChance(float baseChance)
    {
        float modified = baseChance;
        foreach (var effect in effects)
            modified = effect.AdjustHeadsChance(modified);
        return modified;
    }

    // Callback when coin is flipped if we need to do something
    public float ApplyEffect(float value, bool isHeads, CoinManager manager, CoinInstance coinInstance)
    {
        float modified = value;
        foreach (var effect in effects)
            modified = effect.ApplyEffect(modified, isHeads, manager, coinInstance);
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
