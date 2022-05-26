using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RingCountUI : MonoBehaviour
{
    public TMP_InputField inputField;
    public UIAnimation uiAnimation;
    public StartUI startUi;

    private readonly Vector3 _startCamPosition = new Vector3(0, 4, -7);
    private readonly Quaternion _startCamRotation = Quaternion.Euler(Vector3.zero);

    void OnEnable()
    {
        inputField.text = string.Empty;
    }

    public void Accept()
    {
        var parse = int.TryParse(inputField.text, out var ringCount);
        
        if (!parse || ringCount is < 3 or > 10)
        {
            //TODO: Play error
            return;
        }


        Manager.instance.stand.RingsInitiate(ringCount);
        Manager.instance.camController.ChangePositionRotationZoom(_startCamPosition, _startCamRotation, Vector3.zero);
        Manager.instance.camController.enabled = true;

        startUi.active = true;
        startUi.Deactivate();
        uiAnimation.Disable();
        Manager.instance.started = true;
    }
}
