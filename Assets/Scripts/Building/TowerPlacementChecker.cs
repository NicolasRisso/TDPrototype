using UnityEngine;

public class TowerPlacementChecker : MonoBehaviour
{
    public float yTolerance = 1.0f;

    private void Update()
    {
        bool isAbove = IsTowerAboveBuildableArea();

        // Faça algo com o valor de isAbove, como ativar/desativar a colocação da torre
    }

    private bool IsTowerAboveBuildableArea()
    {
        Bounds towerBounds = GetComponent<Collider>().bounds;

        float minY = towerBounds.min.y;

        Vector3 corner1 = new Vector3(towerBounds.min.x, minY, (towerBounds.min.z + towerBounds.max.z) * 0.5f);
        Vector3 corner2 = new Vector3(towerBounds.max.x, minY, (towerBounds.min.z + towerBounds.max.z) * 0.5f);
        Vector3 corner3 = new Vector3((towerBounds.min.x + towerBounds.max.x) * 0.5f, minY, towerBounds.min.z);
        Vector3 corner4 = new Vector3((towerBounds.min.x + towerBounds.max.x) * 0.5f, minY, towerBounds.max.z);


        Debug.DrawRay(corner1, Vector3.down * 10, Color.white);
        Debug.DrawRay(corner2, Vector3.down * 10, Color.white);
        Debug.DrawRay(corner3, Vector3.down * 10, Color.white);
        Debug.DrawRay(corner4, Vector3.down * 10, Color.red);

        bool isAbove = false;
        if (Physics.Raycast(corner4, Vector3.down, yTolerance)) isAbove = true;
        if (Physics.Raycast(corner1, Vector3.down, yTolerance)) isAbove = true;
        if (Physics.Raycast(corner2, Vector3.down, yTolerance)) isAbove = true;
        if (Physics.Raycast(corner3, Vector3.down, yTolerance)) isAbove = true;

        return isAbove;
    }
}
