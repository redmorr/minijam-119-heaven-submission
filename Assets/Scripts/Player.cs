using FMODUnity;
using System;
using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    public float Speed;

    [Header("Weapon")]
    public SpriteRenderer Weapon;
    public Transform WeaponPivot;
    public Transform WeaponMuzzle;
    public AmmoBar ammoBar;
    public Bullet Projectile;
    public EventReference Gunshot;
    public float TimeBetweenShots;
    public float ReloadTime;
    public bool IsReloading = false;
    public EventReference flapWingsSFX;
    public EventReference reloadSFX;
    public Material flashMaterial;


    [Header("Camera")]
    public Transform CameraRoot;
    [Range(0f, 1f)]
    public float MouseWeight = 0.3f;

    private PlayerInputActions playerInputActions;
    private InputAction move;
    private InputAction fire;
    private InputAction look;
    private InputAction reload;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;
    private Material weaponOriginalMaterial;

    //private Coroutine reloadRoutine;

    private Vector2 movement;
    private Vector2 mouse;


    private float gunCooldown = 0f;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ammoBar = GetComponentInChildren<AmmoBar>();
        sprite = GetComponent<SpriteRenderer>();
        weaponOriginalMaterial = Weapon.material;
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

        reload = playerInputActions.Player.Reload;
        reload.Enable();
        reload.started += Reload;
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
            sprite.flipX = true;
            animator.SetBool("Flip", true);
        }
        else
        {
            Weapon.flipY = false;
            sprite.flipX = false;
            animator.SetBool("Flip", false);
        }

        float angle = Vector2.SignedAngle(Vector2.right, toMouse);
        WeaponPivot.eulerAngles = new Vector3(0, 0, angle);


        if (movement != Vector2.zero)
            animator.SetFloat("SpeedMultiplier", 2f);
        else
            animator.SetFloat("SpeedMultiplier", 1f);

        if (gunCooldown > 0f)
        {
            gunCooldown -= Time.deltaTime;
        }
    }


    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + Time.fixedDeltaTime * Speed * movement);
    }

    public void PlayFlapWingsSFX()
    {
        RuntimeManager.PlayOneShot(flapWingsSFX, transform.position);
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (gunCooldown <= 0f && ammoBar.CurrentAmmo > 0 && !IsReloading)
        {
            Bullet bullet = Instantiate(Projectile, WeaponMuzzle.position, WeaponMuzzle.rotation);
            //Instantiate(MuzzleFlash, WeaponMuzzle.position, WeaponMuzzle.rotation);
            RuntimeManager.PlayOneShot(Gunshot, WeaponMuzzle.position);
            bullet.Shoot();
            gunCooldown = TimeBetweenShots;
            if (ammoBar.SpendOneAmmo() <= 0)
            {
                StartCoroutine(ReloadRoutine());
            }
        }
    }

    private void Reload(InputAction.CallbackContext context)
    {
        if (!IsReloading && ammoBar.CurrentAmmo != ammoBar.MaxAmmo)
        {
            StartCoroutine(ReloadRoutine());
        }
    }

    private IEnumerator ReloadRoutine()
    {
        ammoBar.DumpAllAmmo();
        IsReloading = true;
        Weapon.material = flashMaterial;
        RuntimeManager.PlayOneShot(reloadSFX, transform.position);
        yield return new WaitForSeconds(ReloadTime);
        ammoBar.ReloadAllAmmo();
        Weapon.material = weaponOriginalMaterial;
        IsReloading = false;
    }
}
