using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : Health
{
    [Header("Health")]
    public int MaxHP;
    public int HP;
    [Header("Invincibility Frames")]
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

    public UnityAction<int> OnHealthChanged;

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

    public override void Damage(int damage, Vector2 pushback)
    {
        if (!IsInInvincibilityFrame)
        {
            HP -= damage;
            OnHealthChanged?.Invoke(damage);
            if (HP <= 0)
            {
                Destroy(gameObject, 0.5f);
            }

            if (rb)
            {
                rb.AddForce(pushback);
            }

            FlashAndBlink();
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

    private void OnDisable()
    {
        if (flash != null)
        {
            StopCoroutine(flash);
        }
    }
}
