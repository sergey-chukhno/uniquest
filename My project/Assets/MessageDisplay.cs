using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Displays messages on screen for a limited time
/// </summary>
public class MessageDisplay : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI messageText;
    public Image messageBackground;
    
    [Header("Message Settings")]
    public float displayDuration = 3f;
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 0.5f;
    
    [Header("Message Queue")]
    public int maxMessages = 3; // Show up to 3 messages at once
    
    private Queue<string> messageQueue = new Queue<string>();
    private bool isDisplaying = false;
    
    void Start()
    {
        // Hide message initially
        if (messageText != null)
        {
            messageText.text = "";
            messageText.color = new Color(messageText.color.r, messageText.color.g, messageText.color.b, 0f);
        }
        
        if (messageBackground != null)
        {
            messageBackground.color = new Color(messageBackground.color.r, messageBackground.color.g, messageBackground.color.b, 0f);
        }
    }
    
    /// <summary>
    /// Show a message on screen
    /// </summary>
    public void ShowMessage(string message)
    {
        // Add message to queue
        messageQueue.Enqueue(message);
        
        // Start displaying if not already displaying
        if (!isDisplaying)
        {
            StartCoroutine(DisplayMessages());
        }
        
        Debug.Log($"Message queued: {message}");
    }
    
    /// <summary>
    /// Display all queued messages
    /// </summary>
    IEnumerator DisplayMessages()
    {
        isDisplaying = true;
        
        while (messageQueue.Count > 0)
        {
            string message = messageQueue.Dequeue();
            yield return StartCoroutine(ShowSingleMessage(message));
        }
        
        isDisplaying = false;
    }
    
    /// <summary>
    /// Show a single message with fade in/out
    /// </summary>
    IEnumerator ShowSingleMessage(string message)
    {
        // Set message text
        if (messageText != null)
        {
            messageText.text = message;
        }
        
        // Fade in
        yield return StartCoroutine(FadeIn());
        
        // Wait for display duration
        yield return new WaitForSeconds(displayDuration);
        
        // Fade out
        yield return StartCoroutine(FadeOut());
    }
    
    /// <summary>
    /// Fade in the message
    /// </summary>
    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            
            if (messageText != null)
            {
                Color textColor = messageText.color;
                textColor.a = alpha;
                messageText.color = textColor;
            }
            
            if (messageBackground != null)
            {
                Color bgColor = messageBackground.color;
                bgColor.a = alpha * 0.7f; // Background slightly more transparent
                messageBackground.color = bgColor;
            }
            
            yield return null;
        }
    }
    
    /// <summary>
    /// Fade out the message
    /// </summary>
    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        float startAlpha = messageText != null ? messageText.color.a : 0f;
        
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeOutDuration);
            
            if (messageText != null)
            {
                Color textColor = messageText.color;
                textColor.a = alpha;
                messageText.color = textColor;
            }
            
            if (messageBackground != null)
            {
                Color bgColor = messageBackground.color;
                bgColor.a = alpha * 0.7f;
                messageBackground.color = bgColor;
            }
            
            yield return null;
        }
    }
    
    /// <summary>
    /// Clear all messages immediately
    /// </summary>
    public void ClearMessages()
    {
        messageQueue.Clear();
        StopAllCoroutines();
        
        if (messageText != null)
        {
            messageText.text = "";
            Color textColor = messageText.color;
            textColor.a = 0f;
            messageText.color = textColor;
        }
        
        if (messageBackground != null)
        {
            Color bgColor = messageBackground.color;
            bgColor.a = 0f;
            messageBackground.color = bgColor;
        }
        
        isDisplaying = false;
    }
}
