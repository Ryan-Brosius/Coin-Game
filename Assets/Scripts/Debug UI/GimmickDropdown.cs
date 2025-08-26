using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GimmickDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private List<GimmickData> gimmicks;

    void Start()
    {
        FillDropdown();
    }

    private void FillDropdown()
    {
        dropdown.ClearOptions();

        List<string> options = new List<string>();
        foreach (var gimmick in gimmicks)
        {
            options.Add(gimmick.name);
        }

        dropdown.AddOptions(options);
    }

    public GimmickData getGimmick()
    {
        return gimmicks.FirstOrDefault(g => g.name == dropdown.options[dropdown.value].text);
    }
}
