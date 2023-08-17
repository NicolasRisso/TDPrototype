using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float speed = 35f;
    [SerializeField] private bool parabolical = false;

    [Header("Parabolical Movement")]
    [SerializeField] private float initialSpeed = 20f;
    [SerializeField] private float launchAngle = 45f;
    [SerializeField] private float gravity = 9.81f;

    private Transform target;

    private Vector3 initialPosition;
    private Vector3 initialVelocity;

    private float timeSinceLaunch = 0f;

    private void Start()
    {
        if (parabolical)
        {
            initialPosition = transform.position;
            CalculateInitialVelocity();
        }
    }

    public void Seek(Transform _target)
    {
        target = _target;
    }

    private void Update()
    {
        if (target == null) { Destroy(gameObject); return; }
        if (parabolical) ParabolicalTrajectory();
        else LinearTrajectory();
    }

    private void LinearTrajectory()
    {
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        LookAt();
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

    }

    private void ParabolicalTrajectory()
    {
        if (timeSinceLaunch < 0.1f) // Initial delay to avoid inaccuracies
        {
            timeSinceLaunch += Time.deltaTime;
            return;
        }

        float horizontalDistance = initialVelocity.x * timeSinceLaunch;
        float verticalDistance = (initialVelocity.y * timeSinceLaunch) - (0.5f * gravity * timeSinceLaunch * timeSinceLaunch);

        Vector3 newPosition = initialPosition + new Vector3(horizontalDistance, verticalDistance, 0);
        transform.position = newPosition;

        Vector3 nextPosition = initialPosition + new Vector3(horizontalDistance + initialVelocity.x * Time.deltaTime, verticalDistance, initialVelocity.x * Time.deltaTime);
        RotateToDirection(nextPosition - newPosition);

        timeSinceLaunch += Time.deltaTime;
    }

    private void HitTarget()
    {
        Destroy(gameObject);
    }

    private void LookAt()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = lookRotation;
    }

    private void RotateToDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(direction);
            transform.rotation = newRotation;
        }
    }

    private void CalculateInitialVelocity()
    {
        float horizontalSpeed = initialSpeed * Mathf.Cos(launchAngle * Mathf.Deg2Rad);
        float verticalSpeed = initialSpeed * Mathf.Sin(launchAngle * Mathf.Deg2Rad);

        initialVelocity = new Vector3(horizontalSpeed, verticalSpeed, initialSpeed);
    }
}
