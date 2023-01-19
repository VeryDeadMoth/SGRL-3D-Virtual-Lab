using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticHolder : MonoBehaviour
{
    //************************************************************* VARIABLES

    //original placement (si pas placeholder)
    public Vector3 originalPlacement;

    public string containsName; //can be changed to dictionary later if needed

    public float containsQuantity;

    //liquid/solid inside
    //public GameObject objectHeldWithin;

    //shader
    public float fillingValue; //entre 0 et 1 -> quantité de fill

    //demande besoin interaction pour vider
    public bool isRequiringInput;

    //************************************************************* FONCTIONS

    private void Start()
    {
        originalPlacement = transform.position;
    }

}
