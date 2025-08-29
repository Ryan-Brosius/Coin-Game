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
    [SerializeField] ScorePopupController scorePopup;
    [SerializeField] CoinPopupController infoPopup;

    [Header("Animation Values")]
    [SerializeField] float jumpPower = 1;
    [SerializeField] float jumpDuration = 1;
    [SerializeField] float tumblePower = 45;
    [SerializeField] float tumbleDuration = 1f;
    [SerializeField] float rotationNumber = 5f;
    [SerializeField] float coinHeight;

    public enum LandingFace
    {
        Heads,
        Tails,
        Side
    }

    public LandingFace currentFace;

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
        currentFace = LandingFace.Heads;
    }

    private void OnMouseDown()
    {
        if (canToss)
        {
            Transform target = RoundManager.Instance.GetCoinTarget();
            TossCoin(target.position);
            canToss = false;
        }
        else
        {
            RetossCoin();
        }
    }

    // Animation to toss coin
    public void TossCoin(Vector3 target)
    {
        RoundManager.Instance.FlipCoinUsingGameobjectReference(gameObject);
        var coinInstance = RoundManager.Instance.GetCoinInstanceFromGameObject(gameObject);
        SetLandingFace((LandingFace)(int)coinInstance.lastFlippedState);    // XD I LOVE THIS
        CoinRotation(rotationNumber);

        // Jumps to the target location and calls function to rotate to the correct coin face upon landing
        transform.LookAt(target);
        transform.DOJump(target, jumpPower, 1, jumpDuration).SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
                transform.rotation = Quaternion.identity;
                LandCoin();
            });
    }

    public void RetossCoin()
    {
        var coinInstance = RoundManager.Instance.GetCoinInstanceFromGameObject(gameObject);
        SetLandingFace((LandingFace)(int)coinInstance.lastFlippedState);
        FlipInPlaceAnimation();
    }

    // Returns the coin back to the coin bench
    public void ReturnCoin()
    {
        if (transform.position == benchPosition && canToss) return;
        CoinRotation(rotationNumber);
        transform.LookAt(benchPosition);
        transform.DOJump(benchPosition, jumpPower, 1, jumpDuration).SetEase(Ease.OutSine).OnComplete(() =>
        {
            transform.rotation = Quaternion.identity;
            LandOnHeadsNoScore();
            CoinTumble();
        });
        canToss = true;
    }

    private void CoinRotation(float rotations)
    {
        meshTransform.rotation = Quaternion.identity;
        Vector3 targetRotation = Vector3.right * 360f * rotations;
        meshTransform.DOLocalRotate(targetRotation, jumpDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear);
    }

    // Used to set the landing face of the coin
    public void SetLandingFace(LandingFace faceToLand)
    {
        currentFace = faceToLand;
    }

    // Updates the rotation of the coin based on the side it is landing on
    public void LandCoin()
    {
        if (currentFace == LandingFace.Side)
        {
            LandOnSide();
        }
        if (currentFace == LandingFace.Heads)
        {
            LandOnHeads();
            CoinTumble();
        }
        else if (currentFace == LandingFace.Tails)
        {
            LandOnTails();
            CoinTumble();
        }

        RoundManager.Instance.CoinFlipAnimationEnd(gameObject);
    }

    public void CoinTumble()
    {
        meshTransform.DOJump(transform.position, 0.2f, 1, tumbleDuration/4).SetEase(Ease.OutSine)
            .OnComplete(() => {
                meshTransform.DOJump(transform.position, 0.05f, 1, tumbleDuration / 4).SetEase(Ease.OutSine);
            });
        meshTransform.DOShakeRotation(tumbleDuration, tumblePower, 25).SetEase(Ease.OutSine);
    }

    public void CoinStartAnimation()
    {
        gameObject.SetActive(true);
        canToss = true;
        CoinRotation(rotationNumber/2);
        transform.DOJump(transform.position, jumpPower/2, 1, jumpDuration).SetEase(Ease.OutSine).OnComplete(() =>
        {
            LandOnHeadsNoScore();
            CoinTumble();
        });
    }

    private void LandOnHeadsNoScore()
    {
        meshTransform.rotation = Quaternion.Euler(initialRotation.x, initialRotation.y + CoinRotationRandomness(), initialRotation.z);
        if (meshTransform.position.y != 0f) meshTransform.localPosition = new Vector3(0, 0, 0);
    }

    private void LandOnHeads()
    {
        meshTransform.rotation = Quaternion.Euler(initialRotation.x, initialRotation.y + CoinRotationRandomness(), initialRotation.z);
        if (meshTransform.position.y != 0f) meshTransform.localPosition = new Vector3(0, 0, 0);

        if (scorePopup)
        {
            var coinInstance = RoundManager.Instance.GetCoinInstanceFromGameObject(gameObject);
            scorePopup.HeadsPopup(coinInstance?.CoinValue ?? 0f, coinInstance.multiplier?.headsMultiplier ?? 0f, coinInstance?.multiplier ?? false);
        }
    }

    private void LandOnTails()
    {
        meshTransform.rotation = Quaternion.Euler(initialRotation.x + 180, initialRotation.y + CoinRotationRandomness(), initialRotation.z);
        if (meshTransform.position.y != 0f) meshTransform.localPosition = new Vector3(0, 0, 0);

        if (scorePopup)
        {
            var coinInstance = RoundManager.Instance.GetCoinInstanceFromGameObject(gameObject);
            scorePopup.TailsPopup(coinInstance?.CoinValue ?? 0f, coinInstance.multiplier?.tailsMultiplier ?? 0f, coinInstance?.multiplier ?? false);
        }
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

    public bool CanBeTossed()
    {
        return canToss;
    }

    // Prone to breaking? yes, fix later
    public void FlipInPlaceAnimation()
    {
        if (canToss) return;

        CoinRotation(rotationNumber / 2);
        transform.DOJump(transform.position, jumpPower/2, 1, jumpDuration).SetEase(Ease.OutSine).OnComplete(LandCoin);
    }

    public void InitializeCoinInfo()
    {
        if (infoPopup)
        {
            var coinInstance = RoundManager.Instance.GetCoinInstanceFromGameObject(gameObject);
            infoPopup.UpdatePopupInfo(coinInstance);
        }
    }
}
