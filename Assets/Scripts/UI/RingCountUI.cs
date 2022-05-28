using TMPro;
using UnityEngine;

public class RingCountUI : MonoBehaviour
{
    public TMP_InputField playerNameField;
    public TMP_InputField ringCountField;
    public UIAnimation uiAnimation;
    public MenuUI menuUi;
    public GameObject stopwatch;
    public GameObject moveCounter;

    private readonly Vector3 _startCamPosition = new Vector3(0, 4, -7);
    private readonly Quaternion _startCamRotation = Quaternion.Euler(Vector3.zero);

    void OnEnable()
    {
        ringCountField.text = string.Empty;
        playerNameField.text = string.Empty;
    }

    public void Accept()
    {
        if (playerNameField.text.Replace(" ", string.Empty) == string.Empty)
        {
            Manager.instance.audioManager.PlaySfx("error", true, true);
            return;
        }

        var parse = int.TryParse(ringCountField.text, out var ringCount);
        
        if (!parse || ringCount is < 1 or > 10)
        {
            Manager.instance.audioManager.PlaySfx("error", true, true);
            return;
        }


        Manager.instance.stand.RingsInitiate(ringCount);
        Manager.instance.camController.ChangePositionRotationZoom(_startCamPosition, _startCamRotation, Vector3.zero);
        Manager.instance.camController.enabled = true;

        menuUi.active = true;
        menuUi.Deactivate();
        uiAnimation.Disable();
        Manager.instance.started = true;
        stopwatch.SetActive(true);
        moveCounter.SetActive(true);
        try
        {
            Manager.instance.moveCounter.ResetMoves();
        }
        catch
        {
            //ignore
        }
        Manager.instance.playerName = playerNameField.text.Replace(" ", string.Empty).ToLowerInvariant();
        Manager.instance.audioManager.PlaySfx("confirmation", true, true);
    }
}
