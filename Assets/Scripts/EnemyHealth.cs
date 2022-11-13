using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    [Header("Health")]
    public int MaxHP;
    public int HP;
    public int CorpseHP;
    [Header("Flash on Damage")]
    public Material flashMaterial;
    public float flashDuration;
    public EventReference hurtSFX;
    public EventReference deathSFX;


    private Coroutine flash;
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Rigidbody2D rb;
    private Enemy enemy;
    private Animator animator;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
    }

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

        if (HP > 0)
        {
            RuntimeManager.PlayOneShot(deathSFX, transform.position);
            Flash();
        }
        else if (HP == 0)
        {
            enemy.enabled = false;
            animator.enabled = false;
            gameObject.layer = 15; // Corpse
            spriteRenderer.color = new Color(0.2f, 0.2f, 0.2f);
            spriteRenderer.flipY = true;

            if (rb)
            {
                rb.AddForce(pushback);
            }
        }
        else
        {
            if (HP <= -CorpseHP)
            {
                Destroy(gameObject);
            }
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

