using UnityEngine;

public class Interactable : MonoBehaviour
{
    [TextArea]
    public string message = "This is poison";

    public void Interact()
    {
        Debug.Log(message);
    }
}