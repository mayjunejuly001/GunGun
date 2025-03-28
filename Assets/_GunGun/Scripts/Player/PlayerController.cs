using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public GameObject shield;
    private Vector2 move;
    private Vector2 mouseAim;
    private Vector2 joyStickAim;
    private Vector3 rotationTarget;
    public bool isPc;
    private int Score;
    public TextMeshProUGUI scoreText;
    public Image healthImage;

    public event Action onPlayerDies;

    private Health health;
    private CharacterController characterController;

    public bool isShieldActive { get; private set; } = false;
    public float shieldDuration = 10f;  
    public float shieldCooldown = 10f;   
    private bool isShieldOnCooldown = false;

    public void OnMove(InputAction.CallbackContext context) => move = context.ReadValue<Vector2>();

    public void OnMouseLook(InputAction.CallbackContext context) => mouseAim = context.ReadValue<Vector2>();

    public void OnJoyStickLook(InputAction.CallbackContext context) => joyStickAim = context.ReadValue<Vector2>();

    public void OnShield(InputAction.CallbackContext context)
    {
        if (context.performed && !isShieldOnCooldown && !isShieldActive)
        {
            StartCoroutine(ActivateShield());
        }
    }

    private void Awake()
    {
        scoreText.text = "0";
        characterController = GetComponent<CharacterController>();
        health = GetComponent<Health>();
    }

    private void Update()
    {
        if (isPc)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mouseAim);
            if (Physics.Raycast(ray, out hit)) rotationTarget = hit.point;
            movePlayerAim();
        }
        else
        {
            if (joyStickAim == Vector2.zero) movePlayer();
            else movePlayerAim();
        }
    }

    public void movePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y);
        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        }
        characterController.Move(movement * Speed * Time.deltaTime);
    }

    public void movePlayerAim()
    {
        if (isPc)
        {
            Vector3 lookPos = rotationTarget - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);

            if (rotationTarget != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
        }
        else
        {
            Vector3 aimDirection = new Vector3(joyStickAim.x, 0f, joyStickAim.y);
            if (aimDirection != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(aimDirection), 0.15f);
        }

        Vector3 movement = new Vector3(move.x, 0f, move.y);
        characterController.Move(movement * Speed * Time.deltaTime);
    }

    public void takeDamage(float damage)
    {
        if (!isShieldActive)
        {
            health.TakeDamage(damage);
            healthImage.fillAmount = health.gethealthFraction();
            
        }

    }

        IEnumerator ActivateShield()
    {
        isShieldActive = true;
        shield.SetActive(true);

        yield return new WaitForSeconds(shieldDuration); // Wait for shield duration

        isShieldActive = false;
        shield.SetActive(false);
        isShieldOnCooldown = true;

        yield return new WaitForSeconds(shieldCooldown); // Cooldown before reactivating

        isShieldOnCooldown = false;
    }

    private void OnEnable()
    {
        EnemySpawner.onEnemyDie += OnEnemyDead;
        health.OnDeath += Die;
    }

    private void Die()
    {
        onPlayerDies?.Invoke();
    }

    private void OnEnemyDead(EnemyAI aI)
    {
        Score++;
        scoreText.text = Score.ToString();
    }

    private void OnDisable()
    {
        EnemySpawner.onEnemyDie -= OnEnemyDead;
        health.OnDeath -= Die;
    }
}
