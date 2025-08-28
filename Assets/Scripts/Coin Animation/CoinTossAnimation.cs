using DG.Tweening;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CoinTossAnimation : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] Transform meshTransform;
    [SerializeField] Vector3 benchPosition;
    [SerializeField] bool canToss;

    [Header("Animation Values")]
    [SerializeField] float jumpPower = 1;
    [SerializeField] float jumpDuration = 1;
    [SerializeField] float tumblePower = 45;
    [SerializeField] float tumbleDuration = 1f;
    [SerializeField] float rotationNumber = 5f;
    [SerializeField] float coinHeight;

    [Header("Placeholder Stuff")]
    public bool isHeads;
    public bool isSide;

    bool isTossing;
    bool isTumbling;
    Vector3 initialRotation;

    private void Awake()
    {
        if (meshTransform == null) this.transform.Find("Coin Mesh");
        benchPosition = transform.position;
        initialRotation = meshTransform.rotation.eulerAngles;
        gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (canToss)
        {
            Transform target = RoundManager.Instance.GetCoinTarget();
            TossCoin(target.position);
            canToss = false;
        }
    }

    public void TossCoin(Vector3 target)
    {
        Vector3 targetRotation = Vector3.right * 360f * rotationNumber;
        meshTransform.DOLocalRotate(targetRotation, jumpDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear);
        transform.DOJump(target, jumpPower, 1, jumpDuration).SetEase(Ease.OutSine).OnComplete(LandCoin);
    }

    public void ReturnCoin()
    {
        if (transform.position == benchPosition && canToss) return;
        Vector3 targetRotation = Vector3.right * 360f * rotationNumber;
        meshTransform.DOLocalRotate(targetRotation, jumpDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear);
        transform.DOJump(benchPosition, jumpPower, 1, jumpDuration).SetEase(Ease.OutSine).OnComplete(() =>
        {
            LandOnHeads();
            CoinTumble();
        }); ;
        canToss = true;
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
        meshTransform.DOJump(transform.position, 0.1f, 1, tumbleDuration/4).SetEase(Ease.OutSine)
            .OnComplete(() => {
                meshTransform.DOJump(transform.position, 0.05f, 1, tumbleDuration / 4).SetEase(Ease.OutSine);
            });
        meshTransform.DOShakeRotation(tumbleDuration, tumblePower, 25).SetEase(Ease.OutSine);
    }

    public void CoinStartAnimation()
    {
        gameObject.SetActive(true);
        canToss = true;
        Vector3 targetRotation = Vector3.right * 360f * (rotationNumber/2);
        meshTransform.DOLocalRotate(targetRotation, jumpDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear);
        transform.DOJump(transform.position, jumpPower/2, 1, jumpDuration).SetEase(Ease.OutSine).OnComplete(() =>
        {
            LandOnHeads();
            CoinTumble();
        });
    }

    private void LandOnHeads()
    {
        meshTransform.rotation = Quaternion.Euler(initialRotation.x, initialRotation.y + CoinRotationRandomness(), initialRotation.z);
        if (meshTransform.position.y != 0f) meshTransform.localPosition = new Vector3(0, 0, 0);
    }

    private void LandOnTails()
    {
        meshTransform.rotation = Quaternion.Euler(initialRotation.x + 180, initialRotation.y + CoinRotationRandomness(), initialRotation.z);
        if (meshTransform.position.y != 0f) meshTransform.localPosition = new Vector3(0, 0, 0);
    }

    private void LandOnSide()
    {
        meshTransform.localPosition = new Vector3(0f, coinHeight, 0f);
        meshTransform.DOJump(meshTransform.position, 0.5f, 1, tumbleDuration / 4).SetEase(Ease.OutSine)
            .OnComplete(() => {
                meshTransform.DOJump(meshTransform.position, 0.25f, 1, tumbleDuration / 4).SetEase(Ease.OutSine);
            });
        meshTransform.DOLocalRotate(new Vector3(0, 30, 30), tumbleDuration / 3);
    }

    private int CoinRotationRandomness()
    {
        return Random.Range(0, 360);
    }
}
