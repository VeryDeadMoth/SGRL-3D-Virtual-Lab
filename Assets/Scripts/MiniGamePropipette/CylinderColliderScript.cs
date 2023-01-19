using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderColliderScript : MonoBehaviour
{
    public bool isPlaced = false;
    public ScrollPipette scroll;

    private void OnTriggerEnter(Collider other)
    {
        scroll.step = 0.05f;
        isPlaced = true;
        //print("hey");
    }
    private void OnTriggerExit(Collider other)
    {
        scroll.step = 0.1f;
        isPlaced = false;
    }
}
