using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ObjectiveContainsDictionary : Objective
{
    public Dictionary<string, int> dictionaryOfElementsAndQuantityRequired;
    //indique si objectif strict ou non (ou autre)
    public int typeOfContains;

    //Constructeur
    public ObjectiveContainsDictionary(Dictionary<string, int> dictionary, int type)
    {
        this.dictionaryOfElementsAndQuantityRequired = dictionary;
        this.typeOfContains = type;
    }

    public override bool Evaluate(Objective obj)
    {
        if (obj.GetType() == typeof(ObjectiveContainsDictionary))
        {
            ObjectiveContainsDictionary temp = (ObjectiveContainsDictionary)obj;
            if(temp.typeOfContains == 1)
            {
                return TypeOfContainsOne(temp);
            }
            else
            {
                //temp
                return true;
            }
            
        }
        else
        {
            return false;
        }
    }

    //Type strict
    public bool TypeOfContainsOne(ObjectiveContainsDictionary temp)
    {
        bool flag = temp.dictionaryOfElementsAndQuantityRequired.Count == this.dictionaryOfElementsAndQuantityRequired.Count;
        if (flag)
        {
            foreach (KeyValuePair<string, int> pair in this.dictionaryOfElementsAndQuantityRequired)
            {
                
                if (!(temp.dictionaryOfElementsAndQuantityRequired.ContainsKey(pair.Key)) || !(temp.dictionaryOfElementsAndQuantityRequired[pair.Key]==pair.Value))
                {
                    flag = false;
                    break;
                }
            }
        }


        return flag;
    }
}

