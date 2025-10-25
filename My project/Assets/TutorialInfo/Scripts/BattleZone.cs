using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleZone : MonoBehaviour
{
    public int zoneIndex = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player entered zone {zoneIndex}!");
            // For now, just log - we'll add battle scene later
        }
    }
}