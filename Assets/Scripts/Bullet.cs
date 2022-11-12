using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    public int Damage;
    public float PushbackForce;

    private DestructableTilemap tilemap;
    private Health health;

    public void Shoot()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float speed = 10f;
        rb.AddForce(transform.right * speed, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out tilemap))
        {
            tilemap.DestroyTiles(collision);
        }

        if (collision.collider.TryGetComponent(out health))
        {
            health.Damage(Damage, PushbackForce * transform.right);
        }

        Destroy(gameObject);
    }

    //void OnTriggerEnter2D(Collider2D collider)
    //{
    //    Debug.Log(collider.name);
    //    Destroy(gameObject);
    //    //ammoBar.SpendOneAmmo();
    //}
}
