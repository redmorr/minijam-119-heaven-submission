using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
    public float Speed = 0.5f;
    public Transform WeaponPivot;
    public Transform WeaponMuzzle;
    public SpriteRenderer Weapon;
    public Bullet Projectile;
    public float timeBetweenBullets = 1f;
    public LayerMask layerMask;
    public EventReference spawnSFX;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Player player;
    private float cooldown = 1f;

    private Vector2 movement;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(0f, 0f, 1f, 0.5f);
        StartCoroutine(SpoolUp());
    }


    private IEnumerator SpoolUp()
    {
        RuntimeManager.PlayOneShot(spawnSFX);
        yield return new WaitForSeconds(1f);
        animator.enabled = true;
        this.enabled = true;
        spriteRenderer.color = Color.white;
    }

    private void Update()
    {
        Vector2 toPlayer = player.transform.position - transform.position;
        movement = toPlayer.normalized;

        float angle = Vector2.SignedAngle(Vector2.right, toPlayer);
        WeaponPivot.eulerAngles = new Vector3(0, 0, angle);

        float topAngle = Vector2.SignedAngle(Vector2.up, toPlayer);

        if (Mathf.Abs(topAngle) > 30)
            Weapon.sortingOrder = 3;
        else
            Weapon.sortingOrder = 1;

        Weapon.flipY = topAngle > 0;

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), WeaponPivot.right, Mathf.Infinity, layerMask);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player") && cooldown <= 0)
            {
                Bullet bullet = Instantiate(Projectile, WeaponMuzzle.position, WeaponMuzzle.rotation);
                bullet.Shoot();
                cooldown = timeBetweenBullets;
            }
        }


        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            cooldown = 0f;
        }

        animator.SetFloat("X", movement.x);
        animator.SetFloat("Y", movement.y);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + Time.fixedDeltaTime * Speed * movement);
    }
}
