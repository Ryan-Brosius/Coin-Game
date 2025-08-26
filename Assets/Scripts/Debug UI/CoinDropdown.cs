using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private List<CoinData> coins;

    void Start()
    {
        FillDropdown();
    }

    private void FillDropdown()
    {
        dropdown.ClearOptions();

        List<string> options = new List<string>();
        foreach (var coin in coins)
        {
            options.Add(coin.coinName);
        }

        dropdown.AddOptions(options);
    }

    public CoinData getCoin()
    {
        return coins.FirstOrDefault(g => g.name == dropdown.options[dropdown.value].text);
    }
}
