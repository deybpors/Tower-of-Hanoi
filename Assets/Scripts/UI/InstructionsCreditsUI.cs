using TMPro;
using UnityEngine;

public class InstructionsCreditsUI : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI content;
    public GameObject panel;
    [TextArea(3,5)]
    public string creditText;
    [TextArea(3, 5)]
    public string instructionsText;
    private GameObject _thisGameObject;

    public void Credit()
    {
        VerifyThisObject();
        title.text = "Credits";
        content.text = creditText;
        _thisGameObject.SetActive(true);
        panel.SetActive(true);
    }

    public void Instructions()
    {
        VerifyThisObject();
        title.text = "Instructions";
        content.text = instructionsText;
        _thisGameObject.SetActive(true);
        panel.SetActive(true);
    }

    private void VerifyThisObject()
    {
        if (_thisGameObject == null)
        {
            _thisGameObject = gameObject;
        }
    }
}
