using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
    public float Speed = 0.5f;
    public Transform WeaponPivot;
    public Transform WeaponMuzzle;
    public Bullet Projectile;
    public float timeBetweenBullets = 0.5f;
    public LayerMask layerMask;

    private Rigidbody2D rb;
    private Animator animator;

    private Player player;
    private float cooldown;

    private Vector2 movement;

    private void Start()
    {
        player = FindObjectOfType<Player>();

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 toPlayer = player.transform.position - transform.position;
        movement = toPlayer.normalized;

        float angle = Vector2.SignedAngle(Vector2.up, toPlayer);
        WeaponPivot.eulerAngles = new Vector3(0, 0, angle);


        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), WeaponPivot.up, Mathf.Infinity, layerMask);

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
