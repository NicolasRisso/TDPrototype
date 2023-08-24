using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float range = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float fireRate = 1f;

    [Header("TestBase")]
    [SerializeField] private bool isAwake = false;

    [Header("Minor Configurations")]
    [SerializeField] private float angularTolerance = 5f;

    [Header("Tower Demands")]
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private Transform partToRotate;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firepoint;

    private Transform target;

    private float fireCountdown = 0f;
    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
    }

    private void UpdateTarget()
    {
        if (!isAwake) return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            Vector3 positionWithoutY = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 enemyPositionWithoutY = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z);
            float distanceToEnemy = Vector3.Distance(positionWithoutY, enemyPositionWithoutY);
            if (distanceToEnemy < shortestDistance) {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else target = null;
    }

    private void Update()
    {
        if (!isAwake) return;

        fireCountdown -= Time.deltaTime;

        //Security to avoid null targets
        if (target == null) return;

        LockOn();
        if (fireCountdown <= 0f && IsFacingTarget())
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
    }

    private void LockOn()
    {
        //Target lock on
        Vector3 dir = target.position - transform.position;
        dir.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(dir, Vector3.up);
        Quaternion lookRotationWAdjust = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        partToRotate.rotation = lookRotationWAdjust;
    }

    private void Shoot()
    {
        GameObject projectileGO = Instantiate(projectilePrefab, firepoint.position, firepoint.rotation);
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        if (projectile != null) projectile.Target(target);
    }

    private bool IsFacingTarget()
    {
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        float angle = Vector3.Angle(partToRotate.forward, dirToTarget);
        return Mathf.Abs(angle) < angularTolerance;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void SetIsAwake(bool isAwake)
    {
        this.isAwake = isAwake;
    }
}
