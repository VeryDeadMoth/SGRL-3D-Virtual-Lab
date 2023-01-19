using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    float degreesPerSecond = 20;

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButton(1))
        {*/
            
        transform.Rotate(new Vector3(0, degreesPerSecond, 0) * Time.deltaTime);

        //}
    }
}
