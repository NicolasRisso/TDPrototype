using UnityEngine;

public class TowerPainting : MonoBehaviour
{
    private Material originalMaterial;

    private Renderer meshRenderer;

    private void Awake()
    {
        Transform tmp = transform.GetChild(0);
        if (GetComponent<Renderer>() != null)
        {
            meshRenderer = GetComponent<Renderer>();
            originalMaterial = meshRenderer.material;
        }
        else if (tmp.name == "BasePrincipal")
        {
            if (tmp.GetComponent<Renderer>() != null)
            {
                meshRenderer = tmp.GetComponent<Renderer>();
                originalMaterial = meshRenderer.material;
            } else Debug.Log("Null Renderer to BasePrincipal");
        } 
        else Debug.Log("Null Renderer to paint Towers");
    }

    private void ApplyMaterialLoop(Transform transform, Material material)
    {
        foreach (Transform child in transform)
        {
            Paint(child, material);
            if (child.childCount > 0)
            {
                ApplyMaterial(child, material);
            }
        }
    }

    public void ApplyMaterial(Transform transform, Material material)
    {
        //if (meshRenderer.material == material) return;
        ApplyMaterialLoop(transform, material);
    }

    public void PaintBackToOriginal()
    {
        ApplyMaterial(transform, originalMaterial);
    }

    private void Paint(Transform transform, Material material)
    {
        if (transform.GetComponent<Renderer>() != null && transform.name != "Range") transform.GetComponent<Renderer>().material = material;
    }
}
