using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float damage = 5f;
    [SerializeField] private int pierce = 2;
    [SerializeField] private float speed = 35f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float range = 5f;
    [SerializeField] private float projectileMaxDistance = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Extra Stats")]
    [SerializeField] private Preference preference = Preference.First;
    [SerializeField] private bool seekTarget = false;
    [SerializeField] private bool moveYAxis = false;

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
    private Quaternion lookRotationWAdjust;

    private AudioSource audioSource;

    private float fireCountdown = 0f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
    }

    private void UpdateTarget()
    {
        if (!isAwake) return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        float shortestDistance = Mathf.Infinity;
        float mostWalked = Mathf.NegativeInfinity;
        float lessWalked = Mathf.Infinity;
        float biggestDanger = Mathf.NegativeInfinity;

        GameObject preferenceEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();

            Vector3 positionWithoutY = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 enemyPositionWithoutY = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z);
            float distanceToEnemy = Vector3.Distance(positionWithoutY, enemyPositionWithoutY);

            if (distanceToEnemy > range) continue;

            if (preference == Preference.Close)
            {
                if (distanceToEnemy < shortestDistance) {
                    shortestDistance = distanceToEnemy;
                    preferenceEnemy = enemy;
                }
            }
            if (preference == Preference.First)
            {
                if (enemyScript.GetWalkedDistance() > mostWalked)
                {
                    mostWalked = enemyScript.GetWalkedDistance();
                    preferenceEnemy = enemy;
                }
            }
            if (preference == Preference.Last)
            {
                if (enemyScript.GetWalkedDistance() < lessWalked)
                {
                    lessWalked = enemyScript.GetWalkedDistance();
                    preferenceEnemy = enemy;
                }
            }
            if (preference == Preference.Strong)
            {
                if (enemyScript.GetDangerLevel() > biggestDanger)
                {
                    biggestDanger = enemyScript.GetDangerLevel();
                    preferenceEnemy = enemy;
                    if (enemyScript.GetWalkedDistance() > mostWalked)
                    {
                        mostWalked = enemyScript.GetWalkedDistance();
                    }
                }else if (enemyScript.GetDangerLevel() == biggestDanger && enemyScript.GetWalkedDistance() > mostWalked)
                {
                    mostWalked = enemyScript.GetWalkedDistance();
                    preferenceEnemy = enemy;
                }
            }
        }

        if (preferenceEnemy != null)
        {
            target = preferenceEnemy.transform;
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
        lookRotationWAdjust = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        partToRotate.rotation = lookRotationWAdjust;
    }

    private void Shoot()
    {
        GameObject projectileGO = Instantiate(projectilePrefab, firepoint.position, lookRotationWAdjust);
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        audioSource.Play();

        if (projectile != null) projectile.Target(target, damage, pierce, speed, projectileMaxDistance, seekTarget, moveYAxis);
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

    public float GetRange()
    {
        return range;
    }
}
