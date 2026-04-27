using UnityEngine;

public class HangFix : MonoBehaviour
{
    public Transform ropeTop;   // assign rope top
    public Vector3 offset;      // fine adjustment

    void Start()
    {
        transform.position = ropeTop.position + offset;
    }
}