using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 3f;

    public GameObject interactText;   // "Press E"
    public TextMeshProUGUI ClueText;         // "This is poison"

    private Interactable currentTarget;

    void Update()
    {
        CheckTarget();

        if (currentTarget != null)
        {
            interactText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                ShowMessage(currentTarget.message);
            }
        }
        else
        {
            interactText.SetActive(false);
        }
    }

    void CheckTarget()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            currentTarget = hit.collider.GetComponent<Interactable>();
        }
        else
        {
            currentTarget = null;
        }
    }

    void ShowMessage(string msg)
    {
        ClueText.gameObject.SetActive(true);
        ClueText.text = msg;

        CancelInvoke(nameof(HideMessage));
        Invoke(nameof(HideMessage), 3f);
    }

    void HideMessage()
    {
        ClueText.gameObject.SetActive(false);
    }
}