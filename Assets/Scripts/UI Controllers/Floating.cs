using DG.Tweening;
using UnityEngine;

public class Floating : MonoBehaviour
{
    [Header("Float Settings")]
    public float floatDistance = 0.05f;  // How far up/down
    public float floatDuration = 1f;

    private Tween floatingTween;

    private void OnEnable()
    {
        floatingTween = transform.DOMoveY(transform.position.y + floatDistance, floatDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void OnDisable()
    {
        if (floatingTween != null && floatingTween.IsActive())
        {
            floatingTween.Kill();
        }
    }
}
