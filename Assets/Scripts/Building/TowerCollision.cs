using UnityEngine;

public class TowerCollision : MonoBehaviour
{
    private int towersInside = 0;

    public bool IsInsideOtherTower()
    {
        return towersInside > 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower") && other.gameObject != gameObject)
        {
            towersInside++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            towersInside--;
        }
    }
}

