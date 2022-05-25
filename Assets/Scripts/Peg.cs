using System.Collections.Generic;
using UnityEngine;

public class Peg : Entity
{
    public Transform start;
    public Transform end;
    public Transform excess;
    public Dictionary<int, Ring> rings = new Dictionary<int, Ring>();
}
