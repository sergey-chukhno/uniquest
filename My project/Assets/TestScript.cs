using UnityEngine;

public class TestScript : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("TestScript Awake() called!");
    }
    
    void Start()
    {
        Debug.Log("TestScript Start() called!");
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key pressed!");
        }
    }
}
