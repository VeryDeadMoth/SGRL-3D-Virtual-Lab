using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CleanManager : MonoBehaviour
{
    public List<GameObject> listOfObjects; //liste des objets pouvant etre pris aleatoirement
    public int objectCounter; //nombre d'objets pour la scene 
    int selectedObject; //indice de l'objet s�l�ctionn�
    public Camera cam; // la cam

    private int objCount; //actual counter

    //positions
    Vector3 initialPosition = new Vector3(10,2.34f,3.79f);
    Vector3 centralPosition = new Vector3(13.6f, 1.21f, 3.79f);
    Vector3 finalPosition = new Vector3(-10, 2.34f, 3.79f);

    //particules
    public ParticleSystem particles;

    //UI
    public GameObject sliderImage;
    float fillInputPerCleaning; //remplissage de la barre selon le nb d'objet � nettoyer
    public GameObject flecheMinus;
    public GameObject flechePlus;

    //sponge
    public GameObject sponge;

    private void Start()
    {
        //abonnement � event 
        CleaningScript.OnIsDoneCleaningEvent += NextObject;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        //set gauge � 1
        //sliderImage.GetComponent<Image>().fillAmount = 1;

        //indique le fill input pour la barre
        fillInputPerCleaning = 1f / objectCounter;
        //print(fillInputPerCleaning);

        //set obj count
        objCount = objectCounter;

        //par defaut : objets tous desactiv�s, tous plac�s � position initiale
        foreach (GameObject gObj in listOfObjects)
        {
            gObj.SetActive(false);
            gObj.transform.position = initialPosition;
        }

        

        //premier objet activ�
        if (objCount > 0)
        {
            NewObjectToClean();
        }

        //on va au bon endroit 
        cam.GetComponent<CameraManager>().GoToClean();

        // on desactive l'ui non voulue 

        flecheMinus.SetActive(false);
        flechePlus.SetActive(false);

        //[temp] 
        GetComponent<GameManagerPopupTest1>().enabled = false;

        sponge.SetActive(true);

    }

    private void OnDisable()
    {
        sponge.SetActive(false);
    }

    void NextObject()
    {
        //regarde si il y a encore des objets � nettoyer (compteur)
        if(objCount > 1) //si oui (>1 pour que le nb dans objectCounter soit exactement le nb d'objet qui vont s'afficher
        {

            listOfObjects[selectedObject].LeanMove(finalPosition, 1f).setEaseOutQuart();//objet s'en va

            particles.Play(); //activation des particules

            //update de la barre
            LeanTween.value(sliderImage,UpdateValue,sliderImage.GetComponent<Image>().fillAmount, sliderImage.GetComponent<Image>().fillAmount + fillInputPerCleaning, 0.5f).setEaseOutCubic();

            LeanTween.delayedCall(1f, DisableObject); //on desactive l'objet

            objCount--; //decrementation

            LeanTween.delayedCall(1.1f, NewObjectToClean); //appel du nouvel objet

        }
        else //sinon
        {
            listOfObjects[selectedObject].LeanMove(finalPosition,1f).setEaseOutQuart();//objet s'en va

            particles.Play(); //activation des particules

            //update de la barre
            LeanTween.value(sliderImage, UpdateValue, sliderImage.GetComponent<Image>().fillAmount, sliderImage.GetComponent<Image>().fillAmount + fillInputPerCleaning, 0.5f).setEaseOutCubic();

            LeanTween.delayedCall(1f, DisableObject); //on desactive l'objet

            // on desactive le script nettoyage 
            LeanTween.delayedCall(1.5f, DisableSelf);

        }
    }

    void DisableSelf()
    {
        cam.GetComponent<CameraManager>().GoBackClean(); // on retourne � la paillasse 
        //on reactive l'ui 
        flecheMinus.SetActive(true);
        flechePlus.SetActive(true);
        GetComponent<GameManagerPopupTest1>().enabled = true;

        GetComponent<CleanManager>().enabled = false;

    }

    void NewObjectToClean()
    {
        selectedObject = Random.Range(0, listOfObjects.Count); //objet choisi aleatoirement

        listOfObjects[selectedObject].transform.position = initialPosition; //place l'objet en position initiale

        listOfObjects[selectedObject].LeanMove(centralPosition, 1f).setEaseOutQuart();//objet arrive

        listOfObjects[selectedObject].SetActive(true); //objet activ�
    }

    void DisableObject()
    {
        listOfObjects[selectedObject].SetActive(false); //objet desactiv�
        
    }

    public void UpdateValue(float newValue) //utilis� pour l'animation de la barre 
    {
        sliderImage.GetComponent<Image>().fillAmount = newValue;
    }


}
