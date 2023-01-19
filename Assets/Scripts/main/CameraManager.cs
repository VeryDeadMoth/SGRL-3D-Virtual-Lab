using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //positions
    Vector3 balancePosition = new Vector3(10.8f,1.4f,-4.3f);
    Vector3 sorbonnePosition = new Vector3(7.95f,1.7f,-3.7f);
    Vector3 deskPosition = new Vector3(9.8f,1.55f,-3.6f);
    Vector3 cleanPosition = new Vector3(13.6f, 1.3f, 4);

    //rotations
    Vector3 deskRotation = new Vector3(17.3f, 0, 0);
    Vector3 sorbonneRotation = new Vector3(10.7f, 180, 0);
    Vector3 balanceRotation = new Vector3(14.5f, 180, 0);
    Vector3 cleanRotation = new Vector3(18.1f, 180, 0);

    //current placement 
    int currentPlacement; //0 = paillasse / 1 = balance / 2 = sorbonne

    //events
    public delegate void IntEvent(int levelOfSecurity);
    public static event IntEvent OnNewArea; //nouvelle zone



    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = deskPosition; //position paillasse de base
        transform.eulerAngles = deskRotation; //rotation vers paillasse de base
        currentPlacement = 0;
    }

    public void ChangeLocation(int add) //changement location
    {
        currentPlacement += add;
        if(currentPlacement>2 || currentPlacement ==0)
        {
            GoToDesk();
        }
        else if (currentPlacement == 2 || currentPlacement<0)
        {
            GoToSorbonne();
        }
        else if(currentPlacement == 1)
        {
            GoToBalance();
        }
    }

    void GoToSorbonne() //direction sorbonne 
    {
        gameObject.LeanMove(sorbonnePosition, 1f);
        gameObject.LeanRotate(sorbonneRotation, 1f);

        currentPlacement = 2;
        if (OnNewArea != null)
        {
            OnNewArea(currentPlacement); //event
        }
    }
    void GoToBalance() //direction balance 
    {
        gameObject.LeanMove(balancePosition, 1f);
        gameObject.LeanRotate(balanceRotation, 1f);

        currentPlacement = 1;
        if (OnNewArea != null)
        {
            OnNewArea(currentPlacement); //event
        }
    }
    void GoToDesk() //direction paillasse 
    {
        gameObject.LeanMove(deskPosition, 1f);
        gameObject.LeanRotate(deskRotation, 1f);

        currentPlacement = 0;
        if (OnNewArea != null)
        {
            OnNewArea(currentPlacement); //event
        }
    }
    //On s'occupe de la camera du nettoyage 
    public void GoToClean() // direction nettoyage  
    {
        gameObject.LeanMove(cleanPosition, 0.01f);
        gameObject.LeanRotate(cleanRotation, 0.01f);
        GetComponent<Camera>().nearClipPlane = 0.01f;
    }
    public void GoBackClean() // on repart a la paillasse 
    {
        gameObject.LeanMove(deskPosition, 0.01f);
        gameObject.LeanRotate(deskRotation, 0.01f);
        GetComponent<Camera>().nearClipPlane = 0.3f;

        currentPlacement = 0;
        if (OnNewArea != null)
        {
            OnNewArea(currentPlacement); //event
        }
    }

}
