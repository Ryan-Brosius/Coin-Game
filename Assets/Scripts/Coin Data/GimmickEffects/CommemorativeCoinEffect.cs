using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Coins/GimmickEffects/Commemorative Coin")]
public class CommemorativeCoinEffect : GimmickEffect
{
    public float chanceMultiplier;
    private CoinInstance coinCallback = null; // When null not possible to use this
    private int nextCoinIndex = -1;

    public override float ApplyEffect(float value, bool isHeads, CoinManager manager, CoinInstance coinInstance)
    {
        if (isHeads) // Start our callback logic
        {
            // basically all effects that are added to a next coin will stack, so keep using this in the future(?)
            //coinCallback = manager.NextFlippingCoin; // Caused a bug when flipped by large coin(?) might be interesting to play with in the future
            nextCoinIndex = manager.flippedOrder.Count();

            // This would fix logic so when a large coin is flipped but the one above is funnier
            //coinCallback = manager.NextCoinAfterThis(coinInstance);
        }

        return value;
    }

    public override void OnCoinEvent(CoinEventType type, CoinInstance coin, float value, CoinManager manager)
    {
        if (nextCoinIndex == -1)
            return;

        if (nextCoinIndex >= manager.flippedOrder.Count)
            return;

        if (!manager.OnCoinEventCalledFromCurrentFlippingCoin(manager.flippedOrder[nextCoinIndex]))
            return;

        if (type == CoinEventType.BeforeCoinFlip)
        {
            Debug.Log(manager.flippedOrder[nextCoinIndex].CoinName);
            manager.flippedOrder[nextCoinIndex].currentHeadsChance *= chanceMultiplier;
            //coin.currentHeadsChance *= chanceMultiplier;
            //coinCallback = null;
            nextCoinIndex = -1;
        }
    }
}
