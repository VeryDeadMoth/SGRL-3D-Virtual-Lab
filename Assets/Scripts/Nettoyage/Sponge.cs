using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sponge : MonoBehaviour
{
    public ParticleSystem particles;
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0.1f;
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            particles.Play();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            particles.Stop();
        }

    }

}
