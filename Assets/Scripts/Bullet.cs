using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    private DestructableTilemap tilemap;
    private Health health;

    public void Shoot()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float speed = 20f;
        rb.AddForce(transform.up * speed, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out tilemap))
        {
            tilemap.DestroyTiles(collision);
        }

        if (collision.collider.TryGetComponent(out health))
        {
            Debug.Log("Die!");
            health.Damage();
        }

        Destroy(gameObject);
    }
}
