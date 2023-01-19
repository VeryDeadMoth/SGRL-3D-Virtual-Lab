using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comprtementMenisque : MonoBehaviour
{
    public Material matFill;
    public float offset;
    //public Material monMat;
    //Color monMatColor;
    private void FixedUpdate()
    { 
        this.transform.position = new Vector3(this.transform.position.x, (3.45f*matFill.GetFloat("_fill"))+offset+(this.transform.parent.transform.position.y), this.transform.position.z);
        if(getRelativePosition(transform.parent.transform, transform.position).y>1.35f)
        {
            this.transform.position = new Vector3(this.transform.position.x, 5, this.transform.position.z);
        }
        //print(this.transform.position);
    }

    public Vector3 getRelativePosition(Transform origin, Vector3 position)
    {
        Vector3 distance = position - origin.position;
        Vector3 relativePosition = Vector3.zero;
        relativePosition.x = Vector3.Dot(distance, origin.right.normalized);
        relativePosition.y = Vector3.Dot(distance, origin.up.normalized);
        relativePosition.z = Vector3.Dot(distance, origin.forward.normalized);

        return relativePosition;
    }
}
