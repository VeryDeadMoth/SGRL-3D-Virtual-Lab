using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menisqueDisp : MonoBehaviour
{
    public Material mat;
    public GameObject menisque;
    
    // Update is called once per frame
    void Update()
    {
        if (mat.GetFloat("_fill") < 0.03f)
        {
            menisque.SetActive(false);
        }
        else
        {
            menisque.SetActive(true);
        }
    }
}
