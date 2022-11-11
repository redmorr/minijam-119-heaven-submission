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
    private Health playerHealth;


    private void Start()
    {
        playerHealth = FindObjectOfType<Player>().gameObject.GetComponent<Health>();
        MaxHP = playerHealth.MaxHP;
        HP = playerHealth.HP;

        for (int i = 0; i < MaxHP; i++)
        {
            hearts[i].gameObject.SetActive(true);
            hearts[i].color = fullHeartColor;
        }
    }

    private void Update()
    {

    }
}
