using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    public int MaxHP;
    public int HP;
    [Header("Invincibility Frames")]
    public bool TriggerInvincibleAfterDamaged = false;
    public bool IsInInvincibilityFrame = false;
    [Header("Flash on Damage")]
    public Material flashMaterial;
    public float flashDuration;
    [Header("Blink on Invincibility")]
    public int loops;
    public float blinkDuration;

    private Coroutine flash;
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Rigidbody2D rb;

    void Start()
    {
        HP = MaxHP;
        TryGetComponent(out spriteRenderer);
        TryGetComponent(out rb);

        if (spriteRenderer)
        {
            originalMaterial = spriteRenderer.material;
        }
    }

    public void Damage(int damage, Vector2 pushback)
    {
        if (!IsInInvincibilityFrame)
        {
            if (rb)
            {
                rb.AddForce(pushback);
            }

            if (TriggerInvincibleAfterDamaged)
            {
                FlashAndBlink();
            }
            else
            {
                Flash();
            }
        }
    }


    public void FlashAndBlink()
    {
        if (flash != null)
        {
            StopCoroutine(flash);
        }

        flash = StartCoroutine(FlashAndBlinkRoutine());
    }

    private IEnumerator FlashAndBlinkRoutine()
    {
        IsInInvincibilityFrame = true;
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.material = originalMaterial;

        yield return new WaitForSeconds(blinkDuration);

        for (int i = 0; i < loops; i++)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(blinkDuration);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(blinkDuration);
        }

        IsInInvincibilityFrame = false;
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
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.material = originalMaterial;
    }

    private void OnDisable()
    {
        if (flash != null)
        {
            StopCoroutine(flash);
        }
    }
}
