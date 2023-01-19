using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullProtocole_Test_Fiole : Container
{
    public delegate void ObjectHadSomethingHappen(Objective obj);
    public static event ObjectHadSomethingHappen ObjectHadSomethingHappenEvent;

    Dictionary<string, int> dictionaryOfContainedElements = new Dictionary<string, int>();

    public GameObject testLiquid;

    private void Start()
    {
        testLiquid.SetActive(false);
    }

    // Update is called once per frame
    //mouse button 0 -> interaction / prendre
    //mouse button 1 -> ajouter de l'eau distillée (100)
    //mouse button 3 -> empty bottle ?
    void Update()
    {

        if (Input.GetMouseButtonDown(2))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name.Equals(this.name))
            {
                print(hit.transform.name);
                this.dictionaryOfContainedElements.Clear();
                testLiquid.SetActive(false);
                Debug.Log("Cleared");
            }

        }
    }


    public override void ObjectWasFilled()
    {
        ObjectiveContainsDictionary objcd = new ObjectiveContainsDictionary(this.dictionaryOfContainedElements, 1);
        if (ObjectHadSomethingHappenEvent != null)
        {
            ObjectHadSomethingHappenEvent(objcd);
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
        /*ObjectiveGrabItem objgi = new ObjectiveGrabItem(this.tag);
        if (ObjectHadSomethingHappenEvent != null)
        {
            ObjectHadSomethingHappenEvent(objgi);
        }*/

    }

    public override void SelfFill()
    {
        if (this.dictionaryOfContainedElements.ContainsKey("eau distillée"))
        {

            this.dictionaryOfContainedElements["eau distillée"] += 25;
            Debug.Log("+25");
        }
        else
        {
            this.dictionaryOfContainedElements.Add("eau distillée", 25);
            Debug.Log("+25 (set)");
        }
        ObjectWasFilled();

        testLiquid.SetActive(true);
    }

    public override void ClearObject()
    {
        this.dictionaryOfContainedElements.Clear();
        testLiquid.SetActive(false);
    }
}
