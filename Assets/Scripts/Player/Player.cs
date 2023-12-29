using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Cinemachine;
using Inventory.Model;

public class Player : NetworkBehaviour, IDamageable
{
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float runSpeed = 20f;

    private Vector2 axisMovement;

    [SerializeField] float maxStamina = 100f;
    [SerializeField] float stamina;
    [SerializeField] float drainValue;
    [SerializeField] float recoverValue;
    public float staminaPercentage;
    [SerializeField] public Image StaminaBar;
    private Image StaminaFillBar;

    [SerializeField] Image HealthBar;
    private Image HealthFillBar;
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;

    private bool isRunning = false;

    private Rigidbody2D rigidBody;

    [SerializeField] Transform weaponContainer;
    [SerializeField] float offset;

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    public float CurrentHealth { get => health; private set => health = value; }

    public float MaxHealth { get => maxHealth; private set => maxHealth = value; }

    private CinemachineVirtualCamera virtualCamera;

    private InventoryLoader loader;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) this.enabled = false;
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        StaminaFillBar = StaminaBar.transform.GetChild(1).GetComponent<Image>();
        HealthFillBar = HealthBar.transform.GetChild(1).GetComponent<Image>();
        loader = GetComponent<InventoryLoader>();
        loader.LoadItem();
    }

    void Update()
    {
        axisMovement.x = Input.GetAxisRaw("Horizontal");
        axisMovement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (rigidBody.velocity.magnitude > 0)
            {
                isRunning = true;
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }

       /* if (Input.GetKeyDown(KeyCode.Return))
        {
            TakeDamage(20);
        }*/
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Heal(5);
        }

        WeaponRotation();
    }

    private void FixedUpdate()
    {
        TogglePlayerSpeed();
    }

    private void TogglePlayerSpeed()
    {
        if (isRunning)
        {
            RigidbodyVelocity(runSpeed);
            UseStamina();
            StaminaBar.gameObject.SetActive(true);
            if (stamina < 0)
            {
                stamina = 0;
                isRunning = false;
            }
        }
        else
        {
            HealStamina();
            RigidbodyVelocity(walkSpeed);
            if(stamina >= 100)
            {
                StaminaBar.gameObject.SetActive(false);
            }
        }
    }

    private void RigidbodyVelocity(float speed)
    {
        rigidBody.velocity = axisMovement.normalized * speed;
    }

    private void UseStamina()
    {
        stamina -= drainValue * Time.deltaTime;
        UpdateStamina();
    }

    private void HealStamina()
    {
        stamina += recoverValue * Time.deltaTime;
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        UpdateStamina();
    }

    private void UpdateStamina()
    {
        staminaPercentage = stamina / maxStamina;
        StaminaFillBar.fillAmount = staminaPercentage;
    }

    private void WeaponRotation()
    {
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - weaponContainer.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weaponContainer.rotation = Quaternion.Euler(0f, 0f, angle + offset);

        weaponContainer.right = direction;
        Vector2 scale = weaponContainer.localScale;
        if(direction.x < 0)
        {
            scale.y = -1;
        }
        else if(direction.x > 0)
        {
            scale.y = 1;
        }
        weaponContainer.localScale = scale;
    }

    public void TakeDamage(float Damage)
    {
        float damageTaken = Mathf.Clamp(Damage, 0, CurrentHealth);

        CurrentHealth -= damageTaken;

        if (damageTaken != 0)
        {
            OnTakeDamage?.Invoke(damageTaken);
        }

        if (CurrentHealth == 0 && damageTaken != 0)
        {
            OnDeath?.Invoke(transform.position);
        }
        HealthFillBar.fillAmount = health / MaxHealth;
    }

    public void Heal(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, MaxHealth);
        HealthFillBar.fillAmount = health / MaxHealth;
    }
}
