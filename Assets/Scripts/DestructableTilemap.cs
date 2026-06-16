using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(AudioSource))]
public class DestructableTilemap : MonoBehaviour
{
    private Tilemap tilemap;
    private AudioSource audioSource;
    public AudioClip DestructionSFX;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
        audioSource = GetComponent<AudioSource>();
    }

    public void DestroyTiles(Collision2D collision)
    {
        Vector3 hitPosition = Vector3.zero;

        foreach (ContactPoint2D hit in collision.contacts)
        {
            hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
            hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
            tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
        }

        audioSource.PlayOneShot(DestructionSFX);
    }
}
