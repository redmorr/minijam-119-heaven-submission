using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : Health
{
    public SpriteRenderer spriteRenderer;
    public Material originalMaterial;

    private AmmoBar ammoBar;

    private void Awake()
    {
        ammoBar = GetComponentInParent<AmmoBar>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }


    public override void Damage(int damage, Vector2 pushback)
    {
        ammoBar.GotDamaged();
    }

    public void UpdateSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
