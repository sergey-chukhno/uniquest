using UnityEngine;
using System.Collections;

/// <summary>
/// Animates character movement during attacks
/// </summary>
public class AttackAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    public float attackMoveDistance = 0.5f;
    public float attackMoveDuration = 0.2f;
    public float returnDuration = 0.3f;
    
    private Vector3 originalPosition;
    private bool isAnimating = false;
    
    void Start()
    {
        originalPosition = transform.position;
    }
    
    /// <summary>
    /// Play attack animation (move forward and back)
    /// </summary>
    public void PlayAttackAnimation(bool isPlayer)
    {
        if (!isAnimating)
        {
            StartCoroutine(AttackAnimationCoroutine(isPlayer));
        }
    }
    
    IEnumerator AttackAnimationCoroutine(bool isPlayer)
    {
        isAnimating = true;
        originalPosition = transform.position;
        
        // Determine direction (player moves right, enemy moves left)
        float direction = isPlayer ? 1f : -1f;
        Vector3 targetPosition = originalPosition + new Vector3(direction * attackMoveDistance, 0, 0);
        
        // Move forward (attack)
        float elapsed = 0f;
        while (elapsed < attackMoveDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsed / attackMoveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = targetPosition;
        
        // Small pause at peak of attack
        yield return new WaitForSeconds(0.05f);
        
        // Move back to original position
        elapsed = 0f;
        while (elapsed < returnDuration)
        {
            transform.position = Vector3.Lerp(targetPosition, originalPosition, elapsed / returnDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = originalPosition;
        isAnimating = false;
    }
    
    /// <summary>
    /// Play super attack animation (bigger movement, more dramatic)
    /// </summary>
    public void PlaySuperAttackAnimation(bool isPlayer)
    {
        if (!isAnimating)
        {
            StartCoroutine(SuperAttackAnimationCoroutine(isPlayer));
        }
    }
    
    IEnumerator SuperAttackAnimationCoroutine(bool isPlayer)
    {
        isAnimating = true;
        originalPosition = transform.position;
        
        // Bigger movement for super attack
        float direction = isPlayer ? 1f : -1f;
        Vector3 targetPosition = originalPosition + new Vector3(direction * attackMoveDistance * 1.5f, 0, 0);
        
        // Faster, more aggressive movement
        float moveDuration = attackMoveDuration * 0.7f;
        
        // Move forward with slight bounce
        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            // Add bounce effect
            float bounce = Mathf.Sin(t * Mathf.PI) * 0.1f;
            Vector3 pos = Vector3.Lerp(originalPosition, targetPosition, t);
            pos.y += bounce;
            transform.position = pos;
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = targetPosition;
        
        // Longer pause for super attack impact
        yield return new WaitForSeconds(0.1f);
        
        // Return with elastic effect
        elapsed = 0f;
        while (elapsed < returnDuration)
        {
            float t = elapsed / returnDuration;
            // Ease out effect
            float smoothT = 1f - Mathf.Pow(1f - t, 3f);
            transform.position = Vector3.Lerp(targetPosition, originalPosition, smoothT);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = originalPosition;
        isAnimating = false;
    }
}


