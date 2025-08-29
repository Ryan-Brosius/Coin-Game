using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class ScorePopupController : MonoBehaviour
{
    [Header("Animation")]
    public RectTransform startLocation;
    public RectTransform endLocation;
    public float textDuration = 1f;
    public float textDelay = 0.5f;

    [Header ("UI Elements")]
    [SerializeField] TextMeshProUGUI headsText;
    [SerializeField] TextMeshProUGUI headsResult;
    [SerializeField] TextMeshProUGUI tailsText;
    [SerializeField] TextMeshProUGUI tailsResult;

    public void HeadsPopup(float value, float multiplierValue, bool isMultiplier)
    {
        if (isMultiplier)
        {
            headsResult.text = multiplierValue.ToString() + " X";
        }
        else
        {
            headsResult.text = "+" + value.ToString() + " ¢";
        }
        StartCoroutine(PlayHeadsTexts());
    }

    public void TailsPopup(float value, float multiplierValue, bool isMultiplier)
    {
        if (isMultiplier)
        {
            tailsResult.text = multiplierValue.ToString() + " X";
        }
        else
        {
            tailsResult.text = "+" + value.ToString() + " ¢";
        }
        StartCoroutine(PlayTailsTexts());
    }

    IEnumerator PlayHeadsTexts()
    {
        TextAnimation(headsText);
        yield return new WaitForSeconds(textDelay);
        TextAnimation(headsResult);
    }

    IEnumerator PlayTailsTexts()
    {
        TextAnimation(tailsText);
        yield return new WaitForSeconds(textDelay);
        TextAnimation(tailsResult);
    }

    void TextAnimation(TextMeshProUGUI textToAnimate)
    {
        textToAnimate.rectTransform.position = startLocation.position;
        textToAnimate.gameObject.SetActive(true);
        textToAnimate.alpha = 0f;
        textToAnimate.DOFade(1f, 0.1f).SetLoops(3, LoopType.Yoyo).SetEase(Ease.InOutSine)
            .OnComplete(() => textToAnimate.alpha = 1f);
        textToAnimate.rectTransform.DOAnchorPos(endLocation.anchoredPosition, textDuration).SetEase(Ease.InSine)
            .OnComplete(() => {
                textToAnimate.DOFade(0f, textDuration - 0.1f).OnComplete(() => textToAnimate.alpha = 0f);
            }); ;
    }
}
