using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    public TextMeshProUGUI clueText;

    void Awake()
    {
        instance = this;
        clueText.text = "";
    }

    public void ShowText(string message)
    {
        clueText.text = message;
        CancelInvoke();
        Invoke("HideText", 3f);
    }

    void HideText()
    {
        clueText.text = "";
    }
}
