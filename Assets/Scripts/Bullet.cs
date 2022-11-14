using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    public int Damage;
    public int Health;
    public float Speed = 10f;
    public float PushbackForce;
    public GameObject Explosion;

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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 8)
        {
            Instantiate(Explosion, transform.position, Quaternion.identity);
            Destroy(collision.collider.gameObject);
            Destroy(gameObject);
        }
        else if (collision.collider.TryGetComponent(out tilemap))
        {
            tilemap.DestroyTiles(collision);
            Instantiate(Explosion, transform.position, Quaternion.identity);
            Health--;
            if (Health <= 0)
                Destroy(gameObject);
        }
        else if (collision.collider.TryGetComponent(out health))
        {
            health.Damage(Damage, PushbackForce * transform.right);
            Instantiate(Explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
