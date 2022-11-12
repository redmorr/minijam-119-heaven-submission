using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    [Header("Health")]
    public int MaxHP;
    public int HP;
    [Header("Flash on Damage")]
    public Material flashMaterial;
    public float flashDuration;

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

    public override void Damage(int damage, Vector2 pushback)
    {
        HP -= damage;

        if (HP <= 0)
        {
            Destroy(gameObject);
        }

        if (rb)
        {
            rb.AddForce(pushback);
        }

        Flash();
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
