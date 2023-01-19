using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereColliderScript : MonoBehaviour
{
    public Material mat;
    public float fillValue = 0.7f;
    public bool isBroken;
    public bool isFilled;
    public bool isOverFilled;
    // Start is called before the first frame update
    void Start()
    {
        isFilled = false;
        isBroken = false;
        isOverFilled = false;
    }

    private void Update()
    {
        if (mat.GetFloat("_fill") > 0.8f)
        {
            isOverFilled = true;
        }
        else if (mat.GetFloat("_fill") > 0 /*fillValue-0.005 && mat.GetFloat("_fill") < fillValue + 0.005*/)
        {
            isFilled = true;
        }
        else
        {
            isFilled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isBroken = true;
    }

}
