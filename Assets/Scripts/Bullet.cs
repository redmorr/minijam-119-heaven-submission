using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    public int Damage;
    public int Health;
    public float Speed = 10f;
    public float PushbackForce;

    private DestructableTilemap tilemap;
    private Health health;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Shoot()
    {   
        rb.AddForce(transform.right * Speed, ForceMode2D.Impulse);
        Destroy(gameObject, 2f);
    }

    public

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out tilemap))
        {
            tilemap.DestroyTiles(collision);
            Health--;
            if (Health <= 0)
                Destroy(gameObject);
            //rb.AddForce(transform.right * Speed, ForceMode2D.Impulse);
        }

        if (collision.collider.TryGetComponent(out health))
        {
            health.Damage(Damage, PushbackForce * transform.right);
            Destroy(gameObject);
        }

        
    }

    //void OnTriggerEnter2D(Collider2D collider)
    //{
    //    Debug.Log(collider.name);
    //    Destroy(gameObject);
    //    //ammoBar.SpendOneAmmo();
    //}
}
