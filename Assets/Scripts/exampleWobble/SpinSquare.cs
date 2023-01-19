using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinSquare : MonoBehaviour
{
    public Camera cam;
    Vector3 localForward;
    public Quaternion lastRot;
    public float speed;
    public Rigidbody rigidbody;
    public GameObject fiole;
    private float enough = 0;
    public int vit = 36;

    void OnEnable()
    {
        lastRot = transform.rotation;
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            bool hasHit = Physics.Raycast(ray, out hit);


            if (hasHit && hit.collider.gameObject.tag == "Spin")
            {
                var dir = Input.mousePosition - cam.WorldToScreenPoint(transform.position);
                var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
                transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
            }
        }

        if (transform.rotation.z != lastRot.z)
        {
            //print(transform.rotation.z);
            //fiole.transform.rotation = Quaternion.Euler(-120,transform.rotation.z*3600,0);
            fiole.transform.rotation = Quaternion.Euler(-120, transform.rotation.z * vit, 0);
            
            //print("fiole = "+fiole.transform.rotation);
            
            //on stoke a quel poitn ça a bougé 
            enough = enough + Mathf.Abs(transform.rotation.z - lastRot.z);
      
            //Debug.Log(enough);
            

        }
        // si ça a assez bougé on valide le fait que ça soit melangé 
        if (enough > 5)
        {
            fiole.GetComponent<ContainerObjectScript>().wasMixed = true;
        }

        lastRot = transform.rotation;
    }
}
