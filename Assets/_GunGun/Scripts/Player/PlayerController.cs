using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public float Speed;
    private Vector2 move;
    private Vector2 mouseAim;
    private Vector2 joyStickAim;
    private Vector3 rotationTarget;
    public bool isPc;

    private Health health;
    
    private CharacterController characterController;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseAim = context.ReadValue<Vector2>();
    }
    public void OnJoyStickLook(InputAction.CallbackContext context)
    {
        joyStickAim = context.ReadValue<Vector2>();
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        health = GetComponent<Health>();
    }

    private void Update()
    {
        if (isPc)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mouseAim);

            if (Physics.Raycast(ray, out hit))
            {
                rotationTarget = hit.point;
            }

            movePlayerAim();
        }
        else
        {
            if (joyStickAim.x == 0 && joyStickAim.y == 0)
            {
                movePlayer();
            }
            else
            {
                movePlayerAim();
            }
        }
    }


    public void movePlayer()
    {

        Vector3 movement = new Vector3(move.x, 0f, move.y);
        if (movement != Vector3.zero)
        {

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        }
        //transform.Translate(movement * Speed * Time.deltaTime, Space.World);
        characterController.Move(movement * Speed * Time.deltaTime);

    }

    public void movePlayerAim()
    {
        if (isPc)
        {
            var lookPos = rotationTarget - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);

            Vector3 aimDirection = new Vector3(rotationTarget.x, 0f, rotationTarget.z);

            if (aimDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
            }
        }
        else
        {
            Vector3 aimDirection = new Vector3(joyStickAim.x, 0f, joyStickAim.y);
            if (aimDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(aimDirection), 0.15f);
            }

        }

        Vector3 movement = new Vector3(move.x, 0f, move.y);
        //transform.Translate(movement * Speed * Time.deltaTime, Space.World);
        characterController.Move(movement * Speed * Time.deltaTime);
    }

    public void takeDamage(float damage)
    {
        health.TakeDamage(damage);
    }

}
