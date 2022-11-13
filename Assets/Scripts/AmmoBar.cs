using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBar : MonoBehaviour
{
    public Ammo[] ammo;
    public Sprite BulletEmpty;
    public Sprite BulletFull;
    public int CurrentAmmo = 6;
    public int MaxAmmo = 6;
    public EventReference HitSFX;

    public Material flashMaterial;
    public float flashDuration;

    private Coroutine flash;
    private HealthBar healthBar;

    private void Awake()
    {
        healthBar = transform.parent.GetComponent<HealthBar>();
    }

    private void Start()
    {

        foreach (Ammo a in ammo)
        {
            a.UpdateSprite(BulletFull);
        }
    }

    public int SpendOneAmmo()
    {
        if (CurrentAmmo >= 1)
        {
            int newAmmo = CurrentAmmo - 1;
            ammo[newAmmo].UpdateSprite(BulletEmpty);
            CurrentAmmo = newAmmo;
            return CurrentAmmo;
        }
        else
        {
            return 0;
        }
    }

    public void DumpAllAmmo()
    {
        foreach (Ammo a in ammo)
        {
            a.UpdateSprite(BulletEmpty);
        }
        CurrentAmmo = 0;
    }

    public void ReloadAllAmmo()
    {
        foreach (Ammo a in ammo)
        {
            a.UpdateSprite(BulletFull);
        }
        CurrentAmmo = MaxAmmo;
    }

    public void GotDamaged()
    {
        Flash();

        if (CurrentAmmo > 0)
        {
            RuntimeManager.PlayOneShot(HitSFX, transform.position);
            SpendOneAmmo();
        }
        else
        {
            healthBar.Damage(1, Vector2.zero);
        }


    }

    public void Flash()
    {
        if (flash != null)
        {
            StopCoroutine(flash);
        }

        flash = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        foreach (Ammo a in ammo)
        {
            a.spriteRenderer.material = flashMaterial;
        }

        yield return new WaitForSeconds(flashDuration);

        foreach (Ammo a in ammo)
        {
            a.spriteRenderer.material = a.originalMaterial;
        }
    }
}
