using System;
using System.Collections;
using UnityEngine;

public class Ring : Entity
{
    public Peg currentPeg;
    private Peg _pegOnSelectStart;
    private float _movementTime = 1f;
    public int value;

    public override void OnSelectStart()
    {
        _pegOnSelectStart = currentPeg;
        Manager.instance.audioManager.PlaySfx("woodblock", true, true);
    }

    public override void OnSelect()
    {
        if (currentPeg.GetRingAbove(this, out var ringIndex) != null) return;

        HandleRingMovement(out var mousePosition);

        CheckPeg(mousePosition);
    }

    private void CheckPeg(Vector3 mousePosition)
    {
        var stand = Manager.instance.stand;

        for (var i = 0; i < 3; i++)
        {
            if(Math.Abs(mousePosition.x - stand.pegs[i].start.position.x) >= .5f) continue;
            try
            {
                var firstRing = stand.pegs[i].rings[0];
                if (firstRing.value <= value) continue;
                ChangePeg(stand, i);
                break;

            }
            catch
            {
                ChangePeg(stand, i);
                break;
            }
        }
    }

    private void ChangePeg(Stand stand, int i)
    {
        currentPeg.rings.Remove(this);
        stand.pegs[i].rings.Insert(0, this);
        currentPeg = stand.pegs[i];
    }

    private void HandleRingMovement(out Vector3 mousePosition)
    {
        var mainCam = Manager.instance.camController.mainCam;
        var camPosZ = Mathf.Abs(Manager.instance.camController.mainCamera.position.z);
        var mouseInput = new Vector3(Input.mousePosition.x, Input.mousePosition.y, camPosZ);
        mousePosition = mainCam.ScreenToWorldPoint(mouseInput);
        var newPosition = new Vector3(currentPeg.start.position.x, mousePosition.y, currentPeg.start.position.z);
        entityTransform.position = Vector3.Lerp(entityTransform.position, newPosition, Time.deltaTime * _movementTime * 15);
    }

    public override void OnSelectEnd()
    {
        if (_pegOnSelectStart != currentPeg)
        {
            Manager.instance.moveCounter.AddMoves(1);
            Manager.instance.EndCheck();
        }

        StopAllCoroutines();
        StartCoroutine(BackToPosition());
    }

    IEnumerator BackToPosition()
    {
        var ringCount = currentPeg.rings.Count;
        var timeElapsed = 0f;
        currentPeg.GetRingAbove(this, out var ringIndex);

        var sfxPlayed = false;

        while (timeElapsed <= _movementTime)
        {
            timeElapsed += Time.deltaTime;
            var pos = currentPeg.GetRingPositionInPeg(ringCount, ringIndex + 1);
            entityTransform.position = Vector3.Lerp(entityTransform.position, pos, timeElapsed / _movementTime);

            if (Math.Abs(entityTransform.position.y - pos.y) < .5f && !sfxPlayed)
            {
                Manager.instance.audioManager.PlaySfx("woodblock", true, true);
                sfxPlayed = true;
            }

            yield return null;
        }

        StopAllCoroutines();
    }

    public void DisableRing()
    {
        if (entityObject == null)
        {
            entityObject = gameObject;
        }

        currentPeg.rings.Remove(this);
        entityObject.SetActive(false);
    }
}
