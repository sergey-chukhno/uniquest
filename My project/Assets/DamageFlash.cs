using UnityEngine;
using System.Collections;

/// <summary>
/// Flashes the character sprite when taking damage
/// </summary>
public class DamageFlash : MonoBehaviour
{
    [Header("Flash Settings")]
    public float flashDuration = 0.1f;
    public Color flashColor = Color.red;
    public int flashCount = 2;
    
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isFlashing = false;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }
    
    /// <summary>
    /// Trigger damage flash effect
    /// </summary>
    public void Flash()
    {
        if (!isFlashing && spriteRenderer != null)
        {
            StartCoroutine(FlashCoroutine());
        }
    }
    
    IEnumerator FlashCoroutine()
    {
        isFlashing = true;
        
        for (int i = 0; i < flashCount; i++)
        {
            // Flash to damage color
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            
            // Flash back to original
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
        
        // Ensure we end on original color
        spriteRenderer.color = originalColor;
        isFlashing = false;
    }
    
    /// <summary>
    /// Stop flashing and restore original color
    /// </summary>
    public void StopFlash()
    {
        StopAllCoroutines();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
        isFlashing = false;
    }
}


