using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class BiscuitsManager : MonoBehaviour
{
    [SerializeField] GameObject biscuits;
    [SerializeField] Transform biscuitSpawn;
    [SerializeField] GameObject biscuitTin;
    [SerializeField] TextMeshProUGUI biscuitsText;
    [SerializeField] int currentBiscuits;
    [SerializeField] float spawnSpeed = 0.5f;

    public void AddBiscuits(int amount)
    {
        StartCoroutine(BiscuitAddAnimation(amount));
    }

    IEnumerator BiscuitAddAnimation(int amount)
    {
        float relativeSpeed = spawnSpeed;
        for (int i = 0; i < amount; i++)
        {
            GameObject spawn = Instantiate(biscuits, biscuitSpawn.position, Quaternion.identity);
            spawn.transform.DOLocalMoveY(0f, relativeSpeed)
                .OnComplete(() => Destroy(spawn));
            yield return new WaitForSeconds(relativeSpeed * 0.3f);
            currentBiscuits++;
            UpdateBiscuitText();
            yield return new WaitForSeconds(relativeSpeed * 0.7f);
            relativeSpeed *= 0.8f;
        }
    }

    private void UpdateBiscuitText()
    {
        biscuitsText.text = currentBiscuits.ToString() + " BISCS";
        biscuitsText.rectTransform.DOPunchAnchorPos((Vector2.up * 0.1f), 0.1f);
    }

    public int GetCurrentBiscuits()
    {
        return currentBiscuits;
    }

    public bool CanYouAfford(int price)
    {
        if (currentBiscuits >= price) return true;
        else return false;
    }
}
