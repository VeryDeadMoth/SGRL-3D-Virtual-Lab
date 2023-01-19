using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullProtocole_Test_Poudre : Container
{
    public delegate void ObjectHadSomethingHappenPoudre(Objective obj);
    public static event ObjectHadSomethingHappenPoudre ObjectHadSomethingHappenEventPoudre;

    public Dictionary<string, int> dictionaryOfContainedElements = new Dictionary<string, int>();

    public GameObject testLiquid;

    private void Start()
    {
        testLiquid.SetActive(false);
    }

    public override void ObjectWasFilled()
    {
        ObjectiveContainsDictionary objcd = new ObjectiveContainsDictionary(this.dictionaryOfContainedElements, 1);
        if (ObjectHadSomethingHappenEventPoudre != null)
        {
            ObjectHadSomethingHappenEventPoudre(objcd);
        }


    }

    public override void ObjectGotGrabbed()
    {
        /*ObjectiveGrabItem objgi = new ObjectiveGrabItem(this.tag);
        if (ObjectHadSomethingHappenEventPoudre != null)
        {
            ObjectHadSomethingHappenEventPoudre(objgi);
        }*/
    }

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

    public override void SelfFill()
    {
        if (this.dictionaryOfContainedElements.ContainsKey("poudre"))
        {

            this.dictionaryOfContainedElements["poudre"] += 5;
            Debug.Log("+5");
        }
        else
        {
            this.dictionaryOfContainedElements.Add("poudre", 5);
            Debug.Log("+5 (set)");
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
