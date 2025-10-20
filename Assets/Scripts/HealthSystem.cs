using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HealthSystem : MonoBehaviour
{
    [Header("Health")]
    private float health; 
    private float maxHealth = 3f;
    public bool canDie = true, canTakeDamage = true, isAlive = true;
    [NonSerialized] public AttributesSystem attributes;
    private GameObject player;

    [Header("SFX")]
    public string damageSFX;
    public string deathSFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = gameObject;
        if (gameObject.GetComponent<AttributesSystem>() != null) SetMaxHealthAndFullHeal(gameObject.GetComponent<AttributesSystem>().maxHealth.FinalValue);
        else health = maxHealth;
    }

    public void SetMaxHealth(float pMaxHealth)
    {
        maxHealth = pMaxHealth;
    }
    public void SetMaxHealthAndFullHeal(float pMaxHealth)
    {
        maxHealth = pMaxHealth;
        health = maxHealth;
    }

    public void SetHealth(float pHealth)
    {
        health = pHealth;
    }

    public void TakeDamage(float pDamage)
    {
        if (!canTakeDamage || !isAlive) return;

        health = Mathf.Max(health - pDamage, 0);

        if (!string.IsNullOrEmpty(damageSFX)) SFXManager.Instance.Play(damageSFX);

        if (ShouldDie() && canDie == true) Die();
    }

    private void Die()
    {
        if (!string.IsNullOrEmpty(deathSFX)) SFXManager.Instance.Play(deathSFX);

        UIController.Instance.OpenMenu(UIController.Instance.DefeatMenu);
        player.GetComponent<PlayerInput>().actions.FindActionMap("Player").Disable();
    }

    public void HealHealth(float pHealing)
    {
        health = Mathf.Min(health + pHealing, maxHealth);
    }

    public float GetHealth()
    {
        return health;
    }

    public bool ShouldDie()
    {
        return health <= 0;
    }
}
