using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingTool : MonoBehaviour
{
    //************************************************************* VARIABLES

    //original placement (si pas placeholder)
    public Vector3 originalPlacement;

    //contains
    public bool isFull;

    public string containsName; //can be changed to dictionary later if needed

    public float containsQuantity;

    //spoon only ?
    public GameObject objectHeldWithin;

    //shader
    public float fillingValue; //entre 0 et 1 -> quantité de fill

    //demande besoin interaction pour vider
    public bool isRequiringInput;
    public bool asWeight;
    public GameObject source;

    //event -> pour les objectifs
    public delegate void ObjectHadSomethingHappen(Objective objective);
    public static event ObjectHadSomethingHappen ObjectHadSomethingHappenEvent;

    public Dictionary<string, float> elementsContained;

    //************************************************************* FONCTIONS

    private void Start()
    {
        originalPlacement = transform.position;
        if (!isFull)
        {
            objectHeldWithin.SetActive(false);
        }
    }

    public void FillObject(Dictionary<string, float> containerDict) // remplir
    {
        objectHeldWithin.SetActive(true);
        elementsContained = containerDict;
        /*this.containsName = putInName;
        this.containsQuantity = putInQuatity;*/
        this.isFull = true;
    }

    public void EmptyObject() //vider
    {
        objectHeldWithin.SetActive(false);
        /*this.containsName = null;
        this.containsQuantity = 0;*/
        elementsContained.Clear();
        this.isFull = false;
        source = null;
    }

    public void GrabObject()
    {
        ObjectiveGrabItem objgi = new ObjectiveGrabItem(this.name, -1);
        if (ObjectHadSomethingHappenEvent != null)
        {
            ObjectHadSomethingHappenEvent(objgi);
        }
    }

    public void EmptyPipette() //(specifique)
    {
        this.containsName = null;
        this.containsQuantity = 0;
    }

}
