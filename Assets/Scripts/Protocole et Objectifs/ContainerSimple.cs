using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerSimple : Container
{
    public delegate void ObjectHadSomethingHappen(Objective obj);
    public static event ObjectHadSomethingHappen ObjectHadSomethingHappenEvent;

    public Dictionary<string, int> dictionaryOfContainedElements = new Dictionary<string, int>();

    public GameObject testLiquid;
    public int selfFillQuantity;
    public string selfFillElement;

    private void Start()
    {
        testLiquid.SetActive(false);
    }

    public override void ObjectWasFilled()
    {
        ObjectiveContainsDictionary objcd = new ObjectiveContainsDictionary(this.dictionaryOfContainedElements, 1);
        if (ObjectHadSomethingHappenEvent != null)
        {
            ObjectHadSomethingHappenEvent(objcd);
        }
        if (this.dictionaryOfContainedElements.Count != 0)
        {
            testLiquid.SetActive(true);
        }


    }

    //Ajoute un élément et sa quantité dans le dictionnaire puis appelle objectwasfilled
    public override void AddElement(Dictionary<string, int> elementDictionary)
    {
        foreach (KeyValuePair<string, int> pair in elementDictionary)
        {
            if (this.dictionaryOfContainedElements.Count != 0 && this.dictionaryOfContainedElements.ContainsKey(pair.Key))
            {
                this.dictionaryOfContainedElements[pair.Key] += pair.Value;
            }
            else
            {
                this.dictionaryOfContainedElements.Add(pair.Key, pair.Value);
            }
        }

        ObjectWasFilled();

    }

    public override void ObjectGotGrabbed()
    {
        /*ObjectiveGrabItem objgi = new ObjectiveGrabItem(this.name);
        if (ObjectHadSomethingHappenEvent != null)
        {
            ObjectHadSomethingHappenEvent(objgi);
        }*/

    }

    //Remplissage particulié de l'objet (un élément specifique)
    //(remplacer par autre chose plus tard)
    public override void SelfFill()
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
    public override void ClearObject()
    {
        this.dictionaryOfContainedElements.Clear();
        testLiquid.SetActive(false);
    }
}