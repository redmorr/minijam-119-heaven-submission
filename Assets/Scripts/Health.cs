using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Material flashMaterial;
    public float duration;

    private Coroutine flash;
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;

    void Start()
    {
        TryGetComponent(out spriteRenderer);

        if (spriteRenderer)
        {
            originalMaterial = spriteRenderer.material;
        }
    }

    public void Damage()
    {
        Flash();
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
        yield return new WaitForSeconds(duration);

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
