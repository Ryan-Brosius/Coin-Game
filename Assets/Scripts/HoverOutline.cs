using Unity.VisualScripting;
using UnityEngine;

public class HoverOutline : MonoBehaviour
{
    public Material outlineMaterial;
    [SerializeField] CoinTossAnimation animationScript;
    [SerializeField] Renderer coinMesh;
    private Material[] originalMaterials;


    private void Awake()
    {
        originalMaterials = coinMesh.materials;
        if (animationScript == null) animationScript = gameObject.GetComponent<CoinTossAnimation>();
    }

    private void OnMouseEnter()
    {
        ApplyOutline();
    }

    private void OnMouseExit()
    {
        RemoveOutline();
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
}
