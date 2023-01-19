using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CleanlManager : MonoBehaviour
{
    public List<GameObject> listOfObjects; //liste des objets pouvant etre pris aleatoirement
    public int objectCounter; //nombre d'objets pour la scene 
    int selectedObject; //indice de l'objet séléctionné
    

    //positions
    Vector3 initialPosition = new Vector3(10,2.34f,-8);
    Vector3 centralPosition = new Vector3(9.8f, 1.161f, -2.71f);
    //Vector3 centralPosition = new Vector3(0, 2.34f, -8);
    //position finalle Vector3 centralPosition = new Vector3(13.6f, 1.21f, 3.79f);
    Vector3 finalPosition = new Vector3(-10, 2.34f, -8);

    //particules
    public ParticleSystem particles;

    //UI
    public GameObject sliderImage;
    float fillInputPerCleaning; //remplissage de la barre selon le nb d'objet à nettoyer

    // Start is called before the first frame update
    void Start()
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
