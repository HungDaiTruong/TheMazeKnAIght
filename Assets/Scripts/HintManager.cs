using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    public Text hintText;
    private string[] hints = {
        "The maze holds many secrets...",
        "Every turn could lead to freedom...",
        "Watch for patterns in the walls..."
    };

    void Start()
    {
        DisplayRandomHint();
    }

    void DisplayRandomHint()
    {
        hintText.text = hints[Random.Range(0, hints.Length)];
    }
}
