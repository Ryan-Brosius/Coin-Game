using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CoinTossAnimation : MonoBehaviour
{
    [SerializeField] Transform meshTransform;
    [SerializeField] Transform benchPosition;
    [SerializeField] float jumpPower = 1;
    [SerializeField] float jumpDuration = 1;
    [SerializeField] float tumblePower = 45;
    [SerializeField] float tumbleDuration = 1f;
    [SerializeField] float rotationNumber = 5f;
    [SerializeField] float coinHeight;
    public bool isHeads;
    public bool isSide;
    bool isTossing;
    bool isTumbling;
    Vector3 initialRotation;

    public Transform tempTarget;

    private void Awake()
    {
        if (meshTransform == null) this.transform.Find("Coin Mesh");
        initialRotation = meshTransform.rotation.eulerAngles;
    }

    private void OnMouseDown()
    {
        TossCoin(tempTarget.position);
    }

    public void TossCoin(Vector3 target)
    {
        Vector3 targetRotation = Vector3.right * 360f * rotationNumber;
        meshTransform.DOLocalRotate(targetRotation, jumpDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear);
        transform.DOJump(target, jumpPower, 1, jumpDuration).SetEase(Ease.OutSine).OnComplete(LandCoin);
    }

    public void LandCoin()
    {
        if (isSide)
        {
            LandOnSide();
            return;
        }
        if (isHeads)
        {
            LandOnHeads();
            CoinTumble();
        }
        else
        {
            LandOnTails();
            CoinTumble();
        }
    }

    public void CoinTumble()
    {
        meshTransform.DOJump(transform.position, 0.1f, 1, tumbleDuration/3).SetEase(Ease.OutSine);
        meshTransform.DOShakeRotation(tumbleDuration, tumblePower, 25);
    }
    private void LandOnHeads()
    {
        meshTransform.rotation = Quaternion.Euler(initialRotation.x, initialRotation.y, initialRotation.z);
        if (meshTransform.position.y != 0f) meshTransform.localPosition = new Vector3(0, 0, 0);
    }

    private void LandOnTails()
    {
        meshTransform.rotation = Quaternion.Euler(initialRotation.x + 180, initialRotation.y, initialRotation.z);
        if (meshTransform.position.y != 0f) meshTransform.localPosition = new Vector3(0, 0, 0);
    }

    private void LandOnSide()
    {
        meshTransform.localPosition = new Vector3(0f, coinHeight, 0f);
        meshTransform.DOJump(meshTransform.position, 0.1f, 1, tumbleDuration / 3).SetEase(Ease.OutSine);
        meshTransform.rotation = Quaternion.Euler(0, 30, 30);
    }
}
