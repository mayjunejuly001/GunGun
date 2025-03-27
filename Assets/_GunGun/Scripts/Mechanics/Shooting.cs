using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;  // Bullet prefab
    public Transform muzzlePoint;    // Bullet spawn point
    public float bulletSpeed = 20f;  // Bullet velocity
    public float fireRate = 0.2f;    // Time between shots

    private float nextFireTime = 0f;
    private bool isFiring = false;

    private void Update()
    {
        if (isFiring && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        // When Fire button is pressed, set isFiring to true
        if (context.phase == InputActionPhase.Started)
        {
            isFiring = true;
        }
        // When Fire button is released, stop firing
        else if (context.phase == InputActionPhase.Canceled)
        {
            isFiring = false;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = muzzlePoint.forward * bulletSpeed;
        }
    }

    

}
