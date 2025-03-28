using UnityEngine;

public class ExplosiveBullet : MonoBehaviour
{
    
  
    public float explosionRadius = 5f; // Explosion effect radius
    public float explosionDamage = 50f; // Damage dealt in AoE
    public GameObject explosionEffect; // Optional particle effect prefab

    private Rigidbody rb;

    

    void OnCollisionEnter(Collision collision)
    {
        Explode();
        Destroy(gameObject);
    }

    void Explode()
    {
        if (explosionEffect)
        {
            GameObject obj = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            obj.GetComponent<ExplosiveEffect>().setRendererScale(explosionRadius);
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hitColliders)
        {
            if (hit.gameObject.layer != LayerMask.NameToLayer("Player"))
            {
                Health targetHealth = hit.GetComponent<Health>();
                if (targetHealth)
                {
                    targetHealth.TakeDamage(explosionDamage);
                }

            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
