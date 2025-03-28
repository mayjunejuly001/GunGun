using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;  // Bullet prefab
    public GameObject explosiveBulletPrefab;  // Bullet prefab
    public Transform muzzlePoint;    // Bullet spawn point
    public float bulletSpeed = 20f;  // Bullet velocity
    public float fireRate = 0.2f;    // Time between shots
    public float explosiveFireRate = 1f;    // Time between shots
    public float launchAngle = 45f;   // Default launch angle

    private float lastExplosiveFireTime;
    private float nextFireTime = 0f;
    private bool isFiring = false;
    public float launchForce = 20f;

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
    
    public void OnFire2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ShootExplosive();
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
    void ShootExplosive()
    {
       

        if (Time.time - lastExplosiveFireTime >= explosiveFireRate)
        {
            GameObject bullet = Instantiate(explosiveBulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(RotateVector(transform.forward, launchAngle, transform.right).normalized * launchForce , ForceMode.Impulse);
            }
          lastExplosiveFireTime = Time.time;
        }
      
    }

    public static Vector3 RotateVector(Vector3 vector, float angle, Vector3 axis)
    {
        return Quaternion.AngleAxis(angle, axis) * vector;
    }

}
