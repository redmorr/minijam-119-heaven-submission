using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Health
{
    public SpriteRenderer spriteRenderer;
    public Material originalMaterial;

    private HealthBar healthBar;

    private void Awake()
    {
        healthBar = GetComponentInParent<HealthBar>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    public override void Damage(int damage, Vector2 pushback)
    {
        healthBar.ApplyDamage();
    }

    public void UpdateSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

}
