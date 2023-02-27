using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sponge : MonoBehaviour
{
    public GameObject particles;
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0.1f;
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);

    }

    private void OnMouseDown()
    {
        particles.SetActive(true);
        print("a");
    }

    private void OnMouseUp()
    {
        particles.SetActive(false);
    }
}
