using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waipoint : MonoBehaviour
{
    public Waipoint prev, next;
    [Range(0f, 5f)] public float width = 1f;

    public Vector3 getPos()
    {
        Vector3 min = transform.position + transform.right * width / 2f;
        Vector3 max = transform.position - transform.right * width / 2f;
        return Vector3.Lerp(min, max, Random.Range(0f, 1f));
    }
}
