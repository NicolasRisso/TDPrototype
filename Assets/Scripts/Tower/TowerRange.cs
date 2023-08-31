using UnityEngine;

public class TowerRange : MonoBehaviour
{
    [Header("Range Customization")]
    [SerializeField] private float height = 0.25f;
    [SerializeField] private Transform rangeTransform = null;

    private void Awake()
    {
        if (GetComponent<Tower>() != null) SetRange(GetComponent<Tower>().GetRange());
        Enabled(false);
    }

    public void SetRange(float range)
    {
        if (rangeTransform == null) { Debug.Log("No Range Marker at object" + transform.name); return; }
        Vector3 scale = new Vector3(range, height, range);
        rangeTransform.localScale = scale;
    }

    public void Enabled(bool enabled)
    {
        if (rangeTransform == null) { Debug.Log("No Range Marker at object" + transform.name); return; }
        rangeTransform.gameObject.SetActive(enabled);
    }
}
