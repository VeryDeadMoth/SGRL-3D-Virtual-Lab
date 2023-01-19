using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CleanManager : MonoBehaviour
{
    public List<GameObject> listOfObjects; //liste des objets pouvant etre pris aleatoirement
    public int objectCounter; //nombre d'objets pour la scene 
    int selectedObject; //indice de l'objet séléctionné
    public Camera cam; // la cam

    //positions
    Vector3 initialPosition = new Vector3(10,2.34f,3.79f);
    Vector3 centralPosition = new Vector3(13.6f, 1.21f, 3.79f);
    Vector3 finalPosition = new Vector3(-10, 2.34f, 3.79f);

    //particules
    public ParticleSystem particles;

    //UI
    public GameObject sliderImage;
    float fillInputPerCleaning; //remplissage de la barre selon le nb d'objet à nettoyer
    public GameObject flecheMinus;
    public GameObject flechePlus;

    // Start is called before the first frame update
    void OnEnable()
    {
        //set gauge à 1
        //sliderImage.GetComponent<Image>().fillAmount = 1;

        //indique le fill input pour la barre
        fillInputPerCleaning = 1f / objectCounter;
        print(fillInputPerCleaning);

        //par defaut : objets tous desactivés, tous placés à position initiale
        foreach (GameObject gObj in listOfObjects)
        {
            gObj.SetActive(false);
            gObj.transform.position = initialPosition;
        }

        //abonnement à event 
        CleaningScript.OnIsDoneCleaningEvent += NextObject;

        //premier objet activé
        if (objectCounter > 0)
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
    }

    


    void NextObject()
    {
        //regarde si il y a encore des objets à nettoyer (compteur)
        if(objectCounter>1) //si oui (>1 pour que le nb dans objectCounter soit exactement le nb d'objet qui vont s'afficher
        {

            listOfObjects[selectedObject].LeanMove(finalPosition, 0.3f).setEaseOutQuart();//objet s'en va

            particles.Play(); //activation des particules

            //update de la barre
            LeanTween.value(sliderImage,UpdateValue,sliderImage.GetComponent<Image>().fillAmount, sliderImage.GetComponent<Image>().fillAmount + fillInputPerCleaning, 0.2f).setEaseOutCubic();

            LeanTween.delayedCall(0.3f, DisableObject); //on desactive l'objet

            objectCounter--; //decrementation

            LeanTween.delayedCall(0.5f, NewObjectToClean); //appel du nouvel objet

        }
        else //sinon
        {
            listOfObjects[selectedObject].LeanMove(finalPosition, 0.3f).setEaseOutQuart();//objet s'en va

            particles.Play(); //activation des particules

            //update de la barre
            LeanTween.value(sliderImage, UpdateValue, sliderImage.GetComponent<Image>().fillAmount, sliderImage.GetComponent<Image>().fillAmount + fillInputPerCleaning, 0.5f).setEaseOutCubic();

            LeanTween.delayedCall(0.3f, DisableObject); //on desactive l'objet

            cam.GetComponent<CameraManager>().GoBackClean(); // on retounr e a la paillasse 
            //on reactive l'ui 
            flecheMinus.SetActive(true);
            flechePlus.SetActive(true);
            // on desactive le script nettoyage 
            GetComponent<CleanManager>().enabled = false;
            //[temp]
            GetComponent<GameManagerPopupTest1>().enabled = true;

            print("DONE!");
        }
    }

    void NewObjectToClean()
    {
        selectedObject = Random.Range(0, listOfObjects.Count); //objet choisi aleatoirement

        listOfObjects[selectedObject].transform.position = initialPosition; //place l'objet en position initiale

        listOfObjects[selectedObject].LeanMove(centralPosition, 0.3f).setEaseOutQuart();//objet arrive

        listOfObjects[selectedObject].SetActive(true); //objet activé
    }

    void DisableObject()
    {
        listOfObjects[selectedObject].SetActive(false); //objet desactivé
        
    }

    public void UpdateValue(float newValue) //utilisé pour l'animation de la barre 
    {
        sliderImage.GetComponent<Image>().fillAmount = newValue;
    }


}
