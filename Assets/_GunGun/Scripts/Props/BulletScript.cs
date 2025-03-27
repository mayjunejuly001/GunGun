using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float destroyRate = 2f;
    void Start()
    {
        Destroy(gameObject , destroyRate);


    }


    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
