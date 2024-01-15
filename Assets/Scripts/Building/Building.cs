using UnityEngine;

public class Building : MonoBehaviour
{
    [Header("TowerPrefab")]
    [SerializeField] private GameObject towerPreviewPrefab;

    [Header("Key Config")]
    [SerializeField] private KeyCode buildKey;
    [SerializeField] private Material unvalidMaterial;
    [SerializeField] private Material validMaterial;

    [Header("Position Config")]
    [SerializeField] private float yOffSet = 0.15f;
    [SerializeField] private float yTolerance = 0.05f;

    [Header("Pause Menu")]
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private ControlMenu controlsMenu;
    [SerializeField] private ControlMenu creditsMenu;

    private GameObject towerPreview;

    private TowerPainting towerPainting;
    private TowerRange towerRange;

    private bool canBePlaced = false;
    private bool inBuildingMode = false;
    private bool inPause = false;

    private void Update()
    {
        ToggleBuildingMode();

        if (inBuildingMode)
        {
            if (towerPreview == null)
            {
                towerPreview = Instantiate(towerPreviewPrefab);
                if (towerPreview.GetComponent<TowerPainting>() != null) towerPainting = towerPreview.GetComponent<TowerPainting>();
                if (towerPreview.GetComponent<TowerRange>() != null) towerRange = towerPreview.GetComponent<TowerRange>();
                towerPreview.SetActive(false);
            }

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 10f, LayerMask.GetMask("Buildable")))
            {
                towerPreview.SetActive(true);
                towerRange.Enabled(true);
                Vector3 tmp = new Vector3(hitInfo.point.x, hitInfo.point.y + yOffSet, hitInfo.point.z);
                towerPreview.transform.position = tmp;
                towerPreview.transform.rotation = Quaternion.identity; // Ou a rotação desejada
                IsTowerAboveBuildableArea();
                if (canBePlaced) towerPainting.ApplyMaterial(towerPreview.transform, validMaterial);
                else towerPainting.ApplyMaterial(towerPreview.transform, unvalidMaterial);
            }
            //else towerPreview.SetActive(false);
            InputManager();
        }
        else
        {
            if (towerPreview != null)
            {
                Destroy(towerPreview);
            }
        }
    }

    private void InputManager()
    {
        if (Input.GetMouseButtonDown(0) && canBePlaced)
        {
            PlaceTower();
        }
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            inBuildingMode = false;
        }
    }

    private void ToggleBuildingMode()
    {
        if (Input.GetKeyDown(buildKey) && !inPause)
        {
            inBuildingMode = !inBuildingMode;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !inBuildingMode && !inPause){
            controlsMenu.CloseControlMenu();
            creditsMenu.CloseControlMenu();
            pauseMenu.GetComponent<PauseMenu>().Pause(true);
            inPause = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && inPause)
        {
            pauseMenu.Pause(false);
            controlsMenu.CloseControlMenu();
            creditsMenu.CloseControlMenu();
            inPause = false;
        }
    }

    private void PlaceTower()
    {
        if (towerPreview != null)
        {
            GameObject tmp = Instantiate(towerPreviewPrefab, towerPreview.transform.position, towerPreview.transform.rotation);
            tmp.GetComponent<Tower>().SetIsAwake(true);
            towerRange.Enabled(false);
            Destroy(towerPreview);
        }
    }

    private void IsTowerAboveBuildableArea()
    {
        Transform tmp = towerPreview.transform.GetChild(0);
        Bounds towerBounds = tmp.GetComponent<Collider>().bounds;
        TowerCollision towerCollision = tmp.GetComponent<TowerCollision>();

        float minY = towerBounds.min.y;

        Vector3 corner1 = new Vector3(towerBounds.min.x, minY, (towerBounds.min.z + towerBounds.max.z) * 0.5f);
        Vector3 corner2 = new Vector3(towerBounds.max.x, minY, (towerBounds.min.z + towerBounds.max.z) * 0.5f);
        Vector3 corner3 = new Vector3((towerBounds.min.x + towerBounds.max.x) * 0.5f, minY, towerBounds.min.z);
        Vector3 corner4 = new Vector3((towerBounds.min.x + towerBounds.max.x) * 0.5f, minY, towerBounds.max.z);

        if (Physics.Raycast(corner4, Vector3.down, yTolerance) && Physics.Raycast(corner1, Vector3.down, yTolerance) && Physics.Raycast(corner2, Vector3.down, yTolerance) && Physics.Raycast(corner3, Vector3.down, yTolerance) && !towerCollision.IsInsideOtherTower()) canBePlaced = true;
        else canBePlaced = false;
    }
}