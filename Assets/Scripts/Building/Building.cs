using UnityEngine;

public class Building : MonoBehaviour
{
    [Header("TowerPrefab")]
    [SerializeField] private GameObject towerPreviewPrefab;

    [Header("Key Config")]
    [SerializeField] private KeyCode buildKey;

    [Header("Position Config")]
    [SerializeField] private float yOffSet = 0.15f;
    [SerializeField] private float yTolerance = 0.05f;

    private GameObject towerPreview;
    private Collider buildableAreaCollider;

    private bool canBePlaced = false;
    public bool inBuildingMode = false;

    private void Start()
    {
        buildableAreaCollider = GameObject.Find("BuildableArea").GetComponent<Collider>();
    }

    void Update()
    {
        ToggleBuildingMode();

        if (inBuildingMode)
        {
            if (towerPreview == null)
            {
                towerPreview = Instantiate(towerPreviewPrefab);
                towerPreview.SetActive(false);
            }

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 10f, LayerMask.GetMask("Buildable")))
            {
                towerPreview.SetActive(true);
                Vector3 tmp = new Vector3(hitInfo.point.x, hitInfo.point.y + yOffSet, hitInfo.point.z);
                towerPreview.transform.position = tmp;
                towerPreview.transform.rotation = Quaternion.identity; // Ou a rotação desejada
                //Debug.Log(IsTowerAboveBuildableArea());
                //else towerPreview.SetActive(false);
            }
            //else towerPreview.SetActive(false);


            if (Input.GetMouseButtonDown(0) && canBePlaced)
            {
                PlaceTower();
            }
            if (Input.GetMouseButtonDown(1))
            {
                inBuildingMode = false;
            }
        }
        else
        {
            if (towerPreview != null)
            {
                Destroy(towerPreview);
            }
        }
    }

    private void ToggleBuildingMode()
    {
        if (Input.GetKeyDown(buildKey))
        {
            inBuildingMode = !inBuildingMode;
        }
    }

    private void PlaceTower()
    {
        if (towerPreview != null)
        {
            Instantiate(towerPreviewPrefab, towerPreview.transform.position, towerPreview.transform.rotation);
            Destroy(towerPreview);
        }
    }

    private bool IsTowerAboveBuildableArea()
    {
        Bounds towerBounds = towerPreview.transform.GetChild(0).GetComponent<Collider>().bounds;

        float minZ = towerBounds.min.z;

        Vector3 corner1 = new Vector3(towerBounds.min.x, towerBounds.min.x, minZ);
        Vector3 corner2 = new Vector3(towerBounds.max.x, towerBounds.min.x, minZ);
        Vector3 corner3 = new Vector3(towerBounds.max.x, towerBounds.max.x, minZ);
        Vector3 corner4 = new Vector3(towerBounds.min.x, towerBounds.max.x, minZ);

        bool isAbove = true;
        if (Physics.Raycast(corner4, Vector3.down, yTolerance)) isAbove = false;
        if (Physics.Raycast(corner1, Vector3.down, yTolerance)) isAbove = false;
        if (Physics.Raycast(corner2, Vector3.down, yTolerance)) isAbove = false;
        if (Physics.Raycast(corner3, Vector3.down, yTolerance)) isAbove = false;

        return isAbove;
    }



}