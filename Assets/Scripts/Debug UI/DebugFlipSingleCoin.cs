using TMPro;
using UnityEngine;

public class DebugFlipSingleCoin : MonoBehaviour
{
    [SerializeField] private CoinDropdown coinDropdown_obj;
    [SerializeField] private GimmickDropdown gimmickDropdown_obj;
    [SerializeField] private CoinManager coinManager;

    public void FlipSingleCoinButton()
    {
        coinManager.FlipSingleCoin(coinDropdown_obj.getCoin(), null, gimmickDropdown_obj.getGimmick());
    }
}
