using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class TrainingDummy : NetworkBehaviour, IDamageable
{
    [SerializeField] Image HealthBar;
    private Image HealthFillBar;
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    public float CurrentHealth { get => health; private set => health = value; }

    public float MaxHealth { get => maxHealth; private set => maxHealth = value; }

    // Start is called before the first frame update
    void Start()
    {
        HealthFillBar = HealthBar.transform.GetChild(1).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
