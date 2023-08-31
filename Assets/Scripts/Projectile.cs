using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float damage = 5f;
    [SerializeField] private int pierce = 2;
    [SerializeField] private float speed = 35f;
    [SerializeField] private float projectileMaxDistance = 5f;

    [Header("Tweaks")]
    [SerializeField] private bool seekTarget = false;

    [Header("Config")]
    [SerializeField] private bool moveYAxis = false;

    private Transform target;
    private Vector3 targetPosition;

    private bool targetAimed = false;
    private int pierceUsed = 0;
    private float projectileDistance = 0f;

    public void Target(Transform _target, float _damage, int _pierce, float _speed, float _projectileMaxDistance, bool _seekTarget, bool _moveYAxis)
    {
        if (seekTarget) target = _target;
        else targetPosition = _target.position;
        damage = _damage;
        pierce = _pierce;
        speed = _speed;
        projectileMaxDistance = _projectileMaxDistance;
        seekTarget = _seekTarget;
        moveYAxis = _moveYAxis;

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            collider.GetComponent<Health>().Damage(damage);
            pierceUsed++;
            if (pierceUsed >= pierce) Destroy(gameObject);
        }
    }

    private void Update()
    {
        //if (target == null) { Destroy(gameObject); return; }
        LinearTrajectory();
        ProjectileLifetime();
    }

    private void LinearTrajectory()
    {
        float distanceThisFrame = speed * Time.deltaTime;
        Vector3 dir;
        if (seekTarget) { dir = target.position - transform.position; LookAt(); }
        else
        {            
            if (!targetAimed) 
            {
                dir = targetPosition - transform.position;
                LookAt(); 
                targetAimed = true;
            }
            else dir = transform.forward;
        }
        if (!moveYAxis) dir.y = 0;

        projectileDistance += distanceThisFrame; //Contabiliza a distância ja percorrida pelo projetil
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    private void ProjectileLifetime()
    {
        if (projectileDistance >= projectileMaxDistance) Destroy(gameObject);
    }

    private void LookAt()
    {
        Vector3 direct;
        if (target != null) direct = target.position - transform.position; //Difene o alvo que ira mirar.
        else direct = targetPosition - transform.position;
        if (!moveYAxis) direct.y = 0; //Faz com que o projetil não mire no alvo mesmo caso nao deva obedecer ao eixo Y.
        Quaternion lookRotation = Quaternion.LookRotation(direct, Vector3.up);
        transform.rotation = lookRotation;
    }
}
