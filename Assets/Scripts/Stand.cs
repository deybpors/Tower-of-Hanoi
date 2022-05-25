using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stand : Entity
{
    public List<Peg> pegs;
    public float ringOffset;
    public GameObject prefab;
    public float ringAnimationDelay = .25f;
    public Transform ringsParent;
    private Dictionary<int, Ring> _ringsDictionary = new Dictionary<int, Ring>();
    private Dictionary<Ring, Transform> _ringTransforms = new Dictionary<Ring, Transform>();
    private Dictionary<GameObject, ObjectAnimation> _ringAnimations = new Dictionary<GameObject, ObjectAnimation>();
    private HashSet<GameObject> _rings = new HashSet<GameObject>();
    private readonly Vector3 _zero = Vector3.zero;

    public IEnumerator InitiateRings(int ringsCount)
    {
        for (var i = 0; i < ringsCount; i++)
        {
            var ringValue = i + 1;
            GameObject ringObject;

            var ring = SetRing(ringValue, out ringObject, out var ringTransform);

            ringObject.SetActive(false);

            SetPosition(ringsCount, ringValue, ringTransform);

            var newScale = SetScale(i, ringTransform);

            pegs[0].rings.Clear();
            pegs[0].rings.Add(ringValue, ring);

            yield return new WaitForSeconds(ringAnimationDelay);

            SetAnimation(ringObject, newScale);
        }
    }

    public void RingsInitiate(int ringsCount)
    {
        var rings = _ringsDictionary.Values.ToList();
        foreach (var peg in pegs)
        {
            peg.rings.Clear();
        }

        foreach (var ring in rings)
        {
            ring.DisableRing();
        }

        StartCoroutine(InitiateRings(ringsCount));
    }

    private Ring SetRing(int ringValue, out GameObject ringObject, out Transform ringTransform)
    {
        if (!_ringsDictionary.TryGetValue(ringValue, out var ring))
        {
            var obj = Instantiate(prefab, ringsParent);
            ringObject = obj;
            _rings.Add(obj);
            ring = obj.GetComponent<Ring>();
            ring.value = ringValue;
            _ringsDictionary.Add(ringValue, ring);
            _ringAnimations.Add(ringObject, ringObject.GetComponent<ObjectAnimation>());
        }
        else
        {
            ringObject = ring.gameObject;
        }

        if (!_ringTransforms.TryGetValue(ring, out ringTransform))
        {
            ringTransform = ring.transform;
            _ringTransforms.Add(ring, ringTransform);
        }

        ring.currentPeg = pegs[0];

        return ring;
    }

    private void SetAnimation(GameObject ringObject, Vector3 newScale)
    {
        var objectAnimation = (ObjectScale) _ringAnimations[ringObject];
        objectAnimation.start = _zero;
        objectAnimation.end = newScale;
        ringObject.SetActive(true);
    }

    private Vector3 SetScale(int i, Transform ringTransform)
    {
        var newXz = .5f + ((i) * .15f);
        var newScale = new Vector3(newXz, 1, newXz);
        ringTransform.localScale = newScale;
        return newScale;
    }

    private void SetPosition(int ringsCount, int ringValue, Transform ringTransform)
    {
        var newY = pegs[0].start.position.y + ((ringsCount - ringValue) * ringOffset);
        var newPosition = new Vector3(pegs[0].start.position.x, newY, pegs[0].start.position.z);
        ringTransform.position = newPosition;
    }
}
