using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 25f;
    public float lifetime = 5f; // Destroy bullet after 5 sec

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }

        Destroy(gameObject); // Destroy bullet after impact
    }
}
