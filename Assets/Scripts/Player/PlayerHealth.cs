using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] float health;
    [SerializeField] float MaxHealth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            TakeDamage(20);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Heal(5);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / MaxHealth;
    }

    public void Heal(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, MaxHealth);
        healthBar.fillAmount = health / MaxHealth;
    }
}
