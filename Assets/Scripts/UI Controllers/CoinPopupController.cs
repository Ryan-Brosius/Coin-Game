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
            headsText.text = (headsChance).ToString() + " % for " + (coinInfo.multiplier.headsMultiplier).ToString() + "x";
            tailsText.text = (100 - (headsChance)).ToString() + " % for " + (coinInfo.multiplier.tailsMultiplier).ToString() + "x";
        }
        else
        {
            nameText.text = coinInfo.CoinName;
            headsText.text = (headsChance).ToString() + " % for " + "+" + (coinInfo.baseHeadsValue).ToString() + "¢";
            tailsText.text = (100 - (headsChance)).ToString() + " % for " + "+" + (coinInfo.baseTailsValue).ToString() + "¢";
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
