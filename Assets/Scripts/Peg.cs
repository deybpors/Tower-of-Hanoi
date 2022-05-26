using System.Collections.Generic;
using UnityEngine;

public class Peg : Entity
{
    public Stand stand;
    public Transform start;
    public Transform end;
    public Transform excess;
    public List<Ring> rings = new List<Ring>();
    public List<Vector3> positions = new List<Vector3>();

    void Awake()
    {
        for (var i = 0; i < 10; i++)
        {
            var newY = start.position.y + ((10 - (i + 1)) * stand.ringOffset);
            var newPosition = new Vector3(start.position.x, newY, start.position.z);
            positions.Add(newPosition);
        }
    }

    public Ring GetRingAbove(Ring ringToCheck, out int ringToCheckIndex)
    {
        var ringCount = rings.Count;
        Ring ringAbove = null;
        ringToCheckIndex = 0;
        for (var i = 0; i < ringCount; i++)
        {
            if (ringToCheck != rings[i]) continue;

            try
            {
                ringToCheckIndex = i;
                ringAbove = rings[i - 1];
            }
            catch
            {
                //ignored
            }
            break;
        }

        return ringAbove;
    }

    public Vector3 GetRingPositionInPeg(int ringsCount, int ringValue)
    {
        var value = (10 - ringsCount) + (ringValue-1);
        return positions[value];
    }
}
