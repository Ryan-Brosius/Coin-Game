using UnityEngine;

public abstract class GimmickEffect : ScriptableObject
{
    public string _developerNote;   // This is to like add comments in the SO

    public virtual float AdjustHeadsChance(float baseChance) => baseChance;
    public virtual float ApplyEffect(float value, bool isHeads, CoinManager manager) => value;
    public virtual void OnCoinEvent(CoinEventType type, CoinData coin, float value, CoinManager manager) { }
}
