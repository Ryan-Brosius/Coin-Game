using UnityEngine;

public class CashoutBox : MonoBehaviour
{
    private void OnMouseDown()
    {
        RoundManager.Instance.EndRound();
    }
}
