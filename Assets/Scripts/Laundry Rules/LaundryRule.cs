using UnityEngine;

public class LaundryRule : ScriptableObject
{
    [TextArea] public string description;

    // Hooks that we can... hook... into :3
    // Im tired..
    public virtual void OnRoundStart(CoinManager manager) { }
    public virtual void OnBeforeCoinFlip(CoinManager manager, CoinInstance coin) { }
    public virtual void OnAfterCoinFlip(CoinManager manager, CoinInstance coin) { }
    public virtual void OnRoundEnd(CoinManager manager) { }
}
