using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public EnemyType type;
    public PlayerController player;            // Target (player)
    public float detectionRange = 10f;  // Range at which enemy starts chasing
    public float attackRange = 2f;      // Range at which enemy attacks
    public float attackCooldown = 1.5f; // Time between attacks
    public float damage = 20f;
    public Bullet bulletPrefab;

    private NavMeshAgent agent;
    private float nextAttackTime = 0f;
    public Transform muzzlePoint;
    public float bulletSpeed = 10f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();

            if (type == EnemyType.Range)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((player.transform.position - transform.position).normalized) , 0.1f);
            }
        }
        else if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            StopChasing();
        }

    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);
    }

    void StopChasing()
    {
        agent.isStopped = true;
    }

    void EnemyShoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = muzzlePoint.forward * bulletSpeed;
        }
    }
    void AttackPlayer()
    {
        StopChasing();

        if (Time.time >= nextAttackTime)
        {
            if (type == EnemyType.Melee)
            {
             Debug.Log("Enemy attacks!"); // Replace with actual attack logic
             player.takeDamage(damage);
            }
            else
            {
                EnemyShoot();
            }


           nextAttackTime = Time.time + attackCooldown;  
        }
    }
}


public enum EnemyType
{
    Melee , Range
}