using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinSquare : MonoBehaviour
{
    public Camera cam;
    Vector3 localForward;
    Vector3 basePos;
    Quaternion baseRot;
    public Quaternion lastRot;

    public float speed;
    public GameObject fiole;
    public float enough = 0;
    public int vit = 36;

    ContainerObjectScript ctScript;

    void OnEnable()
    {
        basePos = fiole.transform.localPosition;
        baseRot = fiole.transform.localRotation;
        fiole.transform.localPosition = new Vector3(-0.35f, 0.3f, 0.0f);
        lastRot = transform.rotation;

        ctScript = fiole.GetComponent<ContainerObjectScript>();
    }
    private void OnDisable()
    {
        fiole.transform.localPosition = basePos;
        fiole.transform.localRotation = baseRot;
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
                angle += 180;
                transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
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
        }

        
        // si ça a assez bougé on valide le fait que ça soit melangé 
        if (enough > 5 && !ctScript.wasMixed)
        {
            ctScript.wasMixed = true;
            ctScript.Mixed();
        }

        lastRot = transform.rotation;
    }
}
