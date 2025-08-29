using Unity.VisualScripting;
using UnityEngine;

public class HoverOutline : MonoBehaviour
{
    [Header ("Outline")]
    public Material outlineMaterial;
    [SerializeField] CoinTossAnimation animationScript;
    [SerializeField] Renderer coinMesh;
    private Material[] originalMaterials;

    [Header("Popup")]
    [SerializeField] GameObject infoPopup;


    private void Awake()
    {
        originalMaterials = coinMesh.materials;
        if (infoPopup) infoPopup.SetActive(false);
        if (animationScript == null) animationScript = gameObject.GetComponent<CoinTossAnimation>();
    }

    private void OnMouseEnter()
    {
        ApplyOutline();
        ShowPopup();
    }

    private void OnMouseExit()
    {
        RemoveOutline();
        HidePopup();
    }

    void ApplyOutline()
    {
        if (coinMesh == null) return;

        if (animationScript.CanBeTossed())
        {
            // Add outline material to the existing materials
            Material[] newMats = new Material[originalMaterials.Length + 1];
            originalMaterials.CopyTo(newMats, 0);
            newMats[newMats.Length - 1] = outlineMaterial;
            coinMesh.materials = newMats;
        }
    }

    void RemoveOutline()
    {

        if (coinMesh != null && originalMaterials != null)
        {
            coinMesh.materials = originalMaterials;
        }
    }

    void ShowPopup()
    {
        if (animationScript.CanBeTossed())
            infoPopup.SetActive(true);
    }

    void HidePopup()
    {
        infoPopup.SetActive(false);
    }

    public void UpdatePopUpInfo()
    {
        //update the coin info stuff here
    }
}
