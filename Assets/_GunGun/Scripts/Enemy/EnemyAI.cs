using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public EnemyType type;
    public PlayerController player;           
    public float detectionRange = 10f;  
    public float attackRange = 2f;      
    public float attackCooldown = 1.5f; 
    public float meleeDamage = 20f;
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
             Debug.Log("Enemy attacks!"); 
             player.takeDamage(meleeDamage);
            }
            else
            {
                EnemyShoot();
            }


           nextAttackTime = Time.time + attackCooldown;  
        }
    
    }


    

    private void OnDisable()
    {
        if (player!= null)
        {
        player.onPlayerDies -= () => { player = null; };
            
        }
    }


}


public enum EnemyType
{
    Melee , Range
}