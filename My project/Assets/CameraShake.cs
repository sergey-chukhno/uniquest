using UnityEngine;
using System.Collections;

/// <summary>
/// Shakes the camera for impact effects during battle
/// </summary>
public class CameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.1f;
    public float dampingSpeed = 1.0f;
    
    private Vector3 initialPosition;
    private float currentShakeDuration = 0f;
    
    void Start()
    {
        initialPosition = transform.localPosition;
    }
    
    void Update()
    {
        if (currentShakeDuration > 0)
        {
            // Generate random shake offset
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            
            // Decrease shake duration
            currentShakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            // Reset to initial position
            currentShakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }
    
    /// <summary>
    /// Trigger a camera shake effect
    /// </summary>
    public void TriggerShake()
    {
        currentShakeDuration = shakeDuration;
        Debug.Log("Camera shake triggered!");
    }
    
    /// <summary>
    /// Trigger a camera shake with custom intensity
    /// </summary>
    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        currentShakeDuration = duration;
        Debug.Log($"Camera shake triggered! Duration: {duration}, Magnitude: {magnitude}");
    }
}


