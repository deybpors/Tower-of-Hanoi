
using System.Collections.Generic;
using UnityEngine;

public class Ring : Entity
{
    [HideInInspector] public Peg currentPeg;
    public int value;
    private GameObject _thisObject;

    void Start()
    {
        _thisObject = gameObject;
    }

    public void DisableRing()
    {
        currentPeg.rings.Remove(value);
        _thisObject.SetActive(false);
    }
}
