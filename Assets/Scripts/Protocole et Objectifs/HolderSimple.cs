using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderSimple : MonoBehaviour
{
    public delegate void ObjectHadSomethingHappen(Objective obj);
    public static event ObjectHadSomethingHappen ObjectHadSomethingHappenEvent;

    public Dictionary<string, int> dictionaryOfContainedElements = new Dictionary<string, int>();

    public int selfFillQuantity;
    public string selfFillElement;


    public void ObjectWasFilled()
    {
        ObjectiveContainsDictionary objcd = new ObjectiveContainsDictionary(this.dictionaryOfContainedElements, 1);
        if (ObjectHadSomethingHappenEvent != null)
        {
            ObjectHadSomethingHappenEvent(objcd);
        }

    }

    public void ObjectGotGrabbed()
    {
        /*ObjectiveGrabItem objgi = new ObjectiveGrabItem(this.name);
        if (ObjectHadSomethingHappenEvent != null)
        {
            ObjectHadSomethingHappenEvent(objgi);
        }*/

    }

    //Remplissage particulié de l'objet (un élément specifique)
    public void SelfFill()
    {
        if (this.dictionaryOfContainedElements.ContainsKey(selfFillElement))
        {

            this.dictionaryOfContainedElements[selfFillElement] += selfFillQuantity;
            Debug.Log("+" + selfFillQuantity);
        }
        else
        {
            this.dictionaryOfContainedElements.Add(selfFillElement, selfFillQuantity);
            Debug.Log("+" + selfFillQuantity + " set");
        }
        ObjectWasFilled();

    }

    //Vide l'objet
    public void ClearObject()
    {
        this.dictionaryOfContainedElements.Clear();
    }
}
