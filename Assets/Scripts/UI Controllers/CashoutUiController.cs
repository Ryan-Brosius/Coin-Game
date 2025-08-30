using DG.Tweening;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;

public class CashoutUiController : MonoBehaviour
{
    [Header("Cashout Amounts")]
    [SerializeField] int firstCashout = 3;
    [SerializeField] int secondCashout = 5;
    [SerializeField] int finalCashout = 9;
    private cashoutOption currentOption = cashoutOption.Exact;

    [Header("UI Elements")]
    [SerializeField] float inactiveAlpha = 0.2f;
    [SerializeField] TextMeshProUGUI option1;
    [SerializeField] TextMeshProUGUI option2;
    [SerializeField] TextMeshProUGUI option3;
    [SerializeField] GameObject cashoutText;
    [SerializeField] TextMeshProUGUI biscuitsText;
    [HideInInspector]
    public enum cashoutOption
    {
        None,
        First,
        Second,
        Exact
    }
        
    public void UpdateBiscuitsText(int number, bool canCashout)
    {
        if (canCashout)
        {
            cashoutText.SetActive(true);
            biscuitsText.text = number.ToString() + " BISCS";
        }
        else
        {
            cashoutText.SetActive(false);
        }
    }

    public void UpdateCashoutTexts(cashoutOption option)
    {
        if (option == currentOption)
        {
            return;
        }
        if (option == cashoutOption.First)
        {
            currentOption = option;
            TextActive(option1);
            TextInactive(option2);
            TextInactive(option3);
            UpdateBiscuitsText(firstCashout, true);
        }
        else if (option == cashoutOption.Second)
        {
            currentOption = option;
            TextInactive(option1);
            TextActive(option2);
            UpdateBiscuitsText(secondCashout, true);
        }
        else if (option == cashoutOption.Exact)
        {
            currentOption = option;
            TextInactive(option1);
            TextInactive(option2);
            TextActive(option3);
            UpdateBiscuitsText(finalCashout, true);
        }
        else
        {
            currentOption = option;
            TextInactive(option1);
            TextInactive(option2);
            TextInactive(option3);
            UpdateBiscuitsText(0, false);
        }
    }

    public int GetCurrentCashout (cashoutOption option)
    {
        if (option == cashoutOption.Exact) return finalCashout;
        else if (option == cashoutOption.Second) return secondCashout;
        else if (option == cashoutOption.First) return firstCashout;
        else return 0;
    }

    void TextActive(TextMeshProUGUI textToAnimate)
    {
        textToAnimate.gameObject.SetActive(true);
        textToAnimate.alpha = 0f;
        textToAnimate.DOFade(1f, 0.1f).SetLoops(5, LoopType.Yoyo).SetEase(Ease.InOutSine)
            .OnComplete(() => textToAnimate.alpha = 1f);
    }

    void TextInactive(TextMeshProUGUI textToAnimate)
    {
        textToAnimate.DOFade(inactiveAlpha, 0.1f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine)
            .OnComplete(() => textToAnimate.alpha = 0.3f);
    }
}
