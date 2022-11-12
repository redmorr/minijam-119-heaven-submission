using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public SpriteRenderer[] hearts;
    public Color fullHeartColor;
    public Color emptyHeartColor;

    public int MaxHP;
    public int HP;
    private PlayerHealth playerHealth;


    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        MaxHP = playerHealth.MaxHP;
        HP = playerHealth.HP;

        for (int i = 0; i < MaxHP; i++)
        {
            hearts[i].gameObject.SetActive(true);
            hearts[i].color = fullHeartColor;
        }

        playerHealth.OnHealthChanged += UpdateHealthBar;
    }

    private void UpdateHealthBar(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            int index = HP - 1 - i;
            if (index >= 0)
            {
                hearts[HP - 1 - i].color = Color.white;
            }
        }

        HP = playerHealth.HP;
    }

    private void Update()
    {

    }
}
