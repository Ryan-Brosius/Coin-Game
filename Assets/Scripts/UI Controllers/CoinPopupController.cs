using TMPro;
using UnityEngine;

public class CoinPopupController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI headsText;
    [SerializeField] TextMeshProUGUI tailsText;
    [SerializeField] TextMeshProUGUI effectsText;

    public void UpdatePopupInfo(CoinInstance coinInfo)
    {
        var headsChance = CoinManager.Instance.GetCoinChances(coinInfo) * 100f;
        if (coinInfo.multiplier != null)
        {
            headsText.text = (headsChance).ToString() + "% for " + (coinInfo.GetMultiplierValue(true)).ToString() + "x";
            tailsText.text = (100 - (headsChance)).ToString() + "% for " + (coinInfo.GetMultiplierValue(true)).ToString() + "x";
        }
        else
        {
            nameText.text = coinInfo.CoinName;
            headsText.text = (headsChance).ToString() + "% for " + "+" + (coinInfo.GetCoinValue(true)).ToString() + "¢";
            tailsText.text = (100 - (headsChance)).ToString() + "% for " + "+" + (coinInfo.GetCoinValue(false)).ToString() + "¢";
        }
        
        if (coinInfo.gimmick != null)
        {
            effectsText.text = coinInfo.gimmick.gimmickDescription;
        }
        else
        {
            effectsText.text = "";
        }
    }
}
