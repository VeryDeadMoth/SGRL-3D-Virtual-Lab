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
    Vector3 mixPosition = new Vector3(12.61f, 1.55f, -0.5f);

    //rotations
    Vector3 deskRotation = new Vector3(17.3f, 0, 0);
    Vector3 sorbonneRotation = new Vector3(10.7f, 180, 0);
    Vector3 balanceRotation = new Vector3(14.5f, 180, 0);
    Vector3 cleanRotation = new Vector3(18.1f, 180, 0);
    Vector3 mixRotation = new Vector3(17.3f, 180, 0);

    //current placement 
    int currentPlacement; //0 = paillasse / 1 = balance / 2 = sorbonne

    //events
    public delegate void IntEvent(int levelOfSecurity);
    public static event IntEvent OnNewArea; //nouvelle zone
    public LevelOneLevelManager levelManager;
    GameObject objectWasHeld;

    [Header("Music")]
    public AudioSource audioSource;
    public AudioClip gameClip;
    public AudioClip cleanClip;


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
        if (levelManager.objectHeld)
        {
            objectWasHeld = levelManager.objectHeld;
            levelManager.objectHeld = null;
            levelManager.isHolding = false;
            objectWasHeld.transform.parent = transform.parent;
            objectWasHeld.transform.position = objectWasHeld.GetComponent<HoldingTool>().originalPlacement;
            objectWasHeld.GetComponent<HoldingTool>().EmptyObject();
            print(objectWasHeld.transform.position);
            print(objectWasHeld.GetComponent<HoldingTool>().originalPlacement);
        }
        audioSource.clip = cleanClip;
        audioSource.enabled = false;
        audioSource.enabled = true;

        gameObject.LeanMove(cleanPosition, 0.01f);
        gameObject.LeanRotate(cleanRotation, 0.01f);
        GetComponent<Camera>().nearClipPlane = 0.01f;
    }
    public void GoBackClean() // on repart a la paillasse 
    {
        objectWasHeld.transform.position = objectWasHeld.GetComponent<HoldingTool>().originalPlacement;
        objectWasHeld.GetComponent<HoldingTool>().EmptyObject();

        audioSource.clip = gameClip;
        audioSource.enabled = false;
        audioSource.enabled = true;

        gameObject.LeanMove(deskPosition, 0.01f);
        gameObject.LeanRotate(deskRotation, 0.01f);
        GetComponent<Camera>().nearClipPlane = 0.3f;

        currentPlacement = 0;
        if (OnNewArea != null)
        {
            OnNewArea(currentPlacement); //event
        }
    }

    public void GoToMix() // direction nettoyage  
    {
        gameObject.LeanMove(mixPosition, 0.01f);
        gameObject.LeanRotate(mixRotation, 0.01f);
        GetComponent<Camera>().nearClipPlane = 0.01f;
    }
    public void GoBackMix() // on repart a la paillasse 
    {
        gameObject.LeanMove(deskPosition, 0.01f);
        gameObject.LeanRotate(deskRotation, 0.01f);
        GetComponent<Camera>().nearClipPlane = 0.03f;

        currentPlacement = 0;
        if (OnNewArea != null)
        {
            OnNewArea(currentPlacement); //event
        }
    }

}
