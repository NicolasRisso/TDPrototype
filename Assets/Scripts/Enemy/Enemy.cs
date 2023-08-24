using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private int dangerLevel = 1;
    [SerializeField] private float waypointCheckOffset = 0.2f;

    private Transform target;
    private int wavepointIndex = 0;
    private float distanceWalked = 0f;

    private void Start()
    {
        if (Waypoints.points.Length > 0) target = Waypoints.points[0];
        else Debug.Log("No waypoint found");
    }

    private void Update()
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;
        //Movimenta o Inimigo
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        //Atualiza sua visão para o próximo alvo
        if (dir != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = lookRotation;
        }

        distanceWalked += speed * Time.deltaTime;

        if (Mathf.Pow(transform.position.x - target.position.x, 2) + Mathf.Pow(transform.position.z - target.position.z, 2) <= waypointCheckOffset)
        {
            GetNextWaypoint();
        }
    }

    private void GetNextWaypoint()
    {
        //TEMP
        if (wavepointIndex >= Waypoints.points.Length - 1) { Destroy(gameObject); return; }

        wavepointIndex++;
        target = Waypoints.points[wavepointIndex];
    }

    public float GetWalkedDistance()
    {
        return distanceWalked;
    }

    public int GetDangerLevel()
    {
        return dangerLevel;
    }
}
