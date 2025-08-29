using TMPro;
using UnityEngine;

public class CoinPopupController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI headsText;
    [SerializeField] TextMeshProUGUI tailsText;
    [SerializeField] TextMeshProUGUI effectsText;

    public void UpdatePopupInfo(CoinData data)
    {
        if (data.multiplier != null)
        {
            headsText.text = (data.headsChance * 100).ToString() + " % for " + (data.multiplier.headsMultiplier).ToString() + "x";
            tailsText.text = (100 - (data.headsChance * 100)).ToString() + " % for " + "+" + (data.multiplier.tailsMultiplier).ToString() + "x";
        }
        else
        {
            nameText.text = data.coinName;
            headsText.text = (data.headsChance * 100).ToString() + " % for " + "+" + (data.headsValue).ToString() + "¢";
            tailsText.text = (100 - (data.headsChance * 100)).ToString() + " % for " + "+" + (data.tailsValue).ToString() + "¢";
        }
        
        if (data.gimmick != null)
        {
            effectsText.text = data.gimmick.gimmickDescription;
        }
        else
        {
            effectsText.text = "";
        }
    }
}
