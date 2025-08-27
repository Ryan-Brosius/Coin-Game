public enum CoinEventType
{
    BeforeCoinFlip,
    OnFlipStart,      // Before the coin flip happens
    OnFlipEnd,        // After the coin flip resolves its value
    OnAllCoinsFlipped // After the manager finishes all flips
}
