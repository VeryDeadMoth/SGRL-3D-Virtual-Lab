using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ScrollPipette : MonoBehaviour
{
    public SphereColliderScript sphereColliderScript;
    public CylinderColliderScript cylinderColliderScript;
    public TextMeshPro mText;
    public float step = 0.00002f;
    float LastTimeSinceScroll = 0;
    public bool bienPlace =false;
    public bool ICanScroll = true;

    int sensTemp = 1;
    // Start is called before the first frame update
    void Start()
    {
        //transform.position = new Vector3(0,6f,0);
    }
    private void OnEnable()
    {
        bienPlace = false;
        sphereColliderScript.isBroken = false;
        sphereColliderScript.isOverFilled= false;
        LastTimeSinceScroll = 0;
        ICanScroll = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (((Mouse.current.scroll.ReadValue().y >0 && transform.position.y<1.52f) || (Mouse.current.scroll.ReadValue().y < 0 && transform.position.y > 1.43f)) && ICanScroll)
        {
            
            transform.position+= new Vector3(0,step*(Mouse.current.scroll.ReadValue().y/Mathf.Abs(Mouse.current.scroll.ReadValue().y))/10 , 0);
            if (cylinderColliderScript.isPlaced)
            {
                if (Mouse.current.scroll.ReadValue().y < 0)
                {
                    step /= 1.5f; //reduit l'avancé par scroll de la propipette
                }
                else
                {
                    //step *=1.5f;
                }
                LastTimeSinceScroll = Time.realtimeSinceStartup;
            }
            /*if (step < 0.02)                //fair gigoter la propipette si elle n'avance plus assez
            {
                transform.position += new Vector3(0, 0.015f*sensTemp , 0);
                sensTemp *= -1;
            }*/
        }
        

        if (cylinderColliderScript.isPlaced)
        {
            if (!sphereColliderScript.isBroken)
            {
                if (transform.position.y < 1.444)
                {
                    mText.SetText("La propipette est en place");
                    Debug.Log("La propipette est en place");
                    bienPlace = true;
                }

            }
            else
            {
                bienPlace = false;
                mText.SetText("La propipette est cassé");
                Debug.Log(transform.position.y);
                transform.position = new Vector3(transform.position.x, 1.40f, transform.position.z);//se baisse d'un coup si elle est cassé
            }
            
        }
        else
        {
            bienPlace = false;
            mText.SetText("La propipette n'est pas en place");
        }
        if (Time.realtimeSinceStartup - LastTimeSinceScroll > 0.5f) //remet l'avancé par scroll a 0.05 apres avoir atedu une demi seconde.
        {
            step = 0.05f;
        }
        if (sphereColliderScript.isOverFilled)
        {
            bienPlace = false;
        }
    }
}
