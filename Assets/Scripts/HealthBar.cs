using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HealthBar : Health
{
    public Heart[] Hearths;
    public Sprite HeartEmpty;
    public Sprite HeartFull;
    public int CurrentHealth = 6;
    public int MaxHealth = 6;
    public bool IsInInvincibilityFrame = false;
    public Material flashMaterial;
    public float flashDuration;
    public int loops;
    public float blinkDuration;
    public AudioClip HurtSFX;

    private Coroutine flash;
    private Player player;
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Rigidbody2D rb;
    private AudioSource audioSource;

    private void Awake()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        audioSource = GetComponent<AudioSource>();
    }

    public override void Damage(int damage, Vector2 pushback)
    {
        ApplyDamage();
    }

    public void ApplyDamage()
    {
        if (!IsInInvincibilityFrame && CurrentHealth > 0)
        {
            audioSource.PlayOneShot(HurtSFX);
            SpendOneHeart();
            FlashAndBlink();
        }
    }

    public void SpendOneHeart()
    {
        int newHealth = CurrentHealth - 1;
        Hearths[newHealth].UpdateSprite(HeartEmpty);
        CurrentHealth = newHealth;
        if (CurrentHealth == 0)
        {
            player.Die();
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

        foreach (Heart h in Hearths)
            h.spriteRenderer.material = flashMaterial;

        spriteRenderer.material = flashMaterial;

        yield return new WaitForSeconds(flashDuration);

        foreach (Heart h in Hearths)
            h.spriteRenderer.material = h.originalMaterial;

        spriteRenderer.material = originalMaterial;

        yield return new WaitForSeconds(blinkDuration);

        for (int i = 0; i < loops; i++)
        {
            foreach (Heart h in Hearths)
                h.spriteRenderer.color = new Color(1f, 1f, 1f, 0f);

            spriteRenderer.color = new Color(1f, 1f, 1f, 0f);

            yield return new WaitForSeconds(blinkDuration);

            foreach (Heart h in Hearths)
                h.spriteRenderer.color = Color.white;

            spriteRenderer.color = Color.white;

            yield return new WaitForSeconds(blinkDuration);
        }

        IsInInvincibilityFrame = false;
    }
}
