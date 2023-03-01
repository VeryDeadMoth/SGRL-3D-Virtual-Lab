using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunnelScript : MonoBehaviour
{
    public Vector3 originalPlacement;

    private void Start()
    {
        originalPlacement = transform.position;
    }
}
