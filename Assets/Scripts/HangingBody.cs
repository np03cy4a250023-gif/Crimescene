using UnityEngine;

public class HangingAvatar : MonoBehaviour
{
    public GameObject ropeNeck; // Where rope connects
    
    void Start()
    {
        // Freeze the avatar so it doesn't fall
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        
        // Optional: Add slight rotation for realism
        transform.rotation = Quaternion.Euler(0, 0, 3);
    }
}