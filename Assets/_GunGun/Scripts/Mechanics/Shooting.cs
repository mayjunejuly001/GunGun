using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab; 
    public GameObject explosiveBulletPrefab; 
    public Transform muzzlePoint;    
    public float bulletSpeed = 20f;  
    public float fireRate = 0.2f;   
    public float explosiveFireRate = 1f;    
    public float launchAngle = 45f;  

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
        
        if (context.phase == InputActionPhase.Started)
        {
            isFiring = true;
        }
       
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
