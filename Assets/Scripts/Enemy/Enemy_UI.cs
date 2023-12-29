using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy_UI : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] TMP_Text nameBar;
    [SerializeField] Vector3 offsetHealth;
    [SerializeField] Vector3 offsetName;

    private void Start()
    {
        healthBar.gameObject.SetActive(true);
        nameBar.gameObject.SetActive(true);
    }

    void Update()
    {
        healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offsetHealth);
        nameBar.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offsetName);
    }

    public void SetHealth(float health, float maxHealth)
    {
        healthBar.value = health;
        healthBar.maxValue = maxHealth;
    }
}
