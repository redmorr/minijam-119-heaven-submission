using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class Player : MonoBehaviour
{
    public float Speed;

    [Header("Weapon")]
    public SpriteRenderer Weapon;
    public Transform WeaponPivot;
    public Transform WeaponMuzzle;
    public Bullet Projectile;
    public EventReference Gunshot;

    [Header("Ammo")]
    public AmmoBar ammoBar;


    [Header("Camera")]
    public Transform CameraRoot;
    [Range(0f, 1f)]
    public float MouseWeight = 0.3f;

    private PlayerInputActions playerInputActions;
    private InputAction move;
    private InputAction fire;
    private InputAction look;
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 movement;
    private Vector2 mouse;

    private Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, 0f, 0f));

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ammoBar = GetComponentInChildren<AmmoBar>();
    }

    void OnEnable()
    {
        move = playerInputActions.Player.Move;
        move.Enable();

        look = playerInputActions.Player.Look;
        look.Enable();

        fire = playerInputActions.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
    }

    private void OnDisable()
    {
        move.Disable();
        fire.Disable();
        look.Disable();
    }

    private void Update()
    {
        movement = move.ReadValue<Vector2>();
        mouse = look.ReadValue<Vector2>();

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(mouse);
        mousePos.z = 0;

        Vector2 toMouse = mousePos - transform.position;
        CameraRoot.position = transform.position + 0.2f * new Vector3(toMouse.x, toMouse.y, 0f);

        float topAngle = Vector2.SignedAngle(Vector2.up, toMouse);

        if (Mathf.Abs(topAngle) > 30)
            Weapon.sortingOrder = 3;
        else
            Weapon.sortingOrder = 1;

        if (topAngle > 0)
        {
            Weapon.flipY = true;
            animator.SetBool("Flip", true);
        }
        else
        {
            Weapon.flipY = false;
            animator.SetBool("Flip", false);
        }

        float angle = Vector2.SignedAngle(Vector2.right, toMouse);
        WeaponPivot.eulerAngles = new Vector3(0, 0, angle);

        /* human animator
        animator.SetFloat("X", movement.x);
        animator.SetFloat("Y", movement.y);
        */
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + Time.fixedDeltaTime * Speed * movement);
    }

    private void Fire(InputAction.CallbackContext context)
    {
        Bullet bullet = Instantiate(Projectile, WeaponMuzzle.position, WeaponMuzzle.rotation);
        //Instantiate(MuzzleFlash, WeaponMuzzle.position, WeaponMuzzle.rotation);
        RuntimeManager.PlayOneShot(Gunshot, WeaponMuzzle.position);
        bullet.Shoot();
    }
}
