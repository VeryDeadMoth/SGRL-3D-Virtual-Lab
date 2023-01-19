using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveContainsStrict : Objective

    //VERSION OBJECTIVECONTAINS AVEC PARAMA INT QUI INDIQUE TYPE D'OBJECTIVE CONTAINS (si strict ou non.. conditions,etc)
{
    //liste de couples (élément,quantité) que l'on veut / que l'on a
    List<(string, int)> listOfElementsAndQuantityRequired;

    //Constructeur
    public ObjectiveContainsStrict(List<(string, int)> list)
    {
        this.listOfElementsAndQuantityRequired = list;
    }

    //Verification du type de l'objectif reçu puis verification des elements de la liste de l'objectif reçu
    //Note : verifie que tous les elements voulus sont dans l'objet (et que d'autres elements n'y sont pas)
    public override bool Evaluate(Objective obj)
    {
        if (obj.GetType() == typeof(ObjectiveContainsStrict))
        {
            ObjectiveContainsStrict temp = (ObjectiveContainsStrict)obj;
            if(temp.listOfElementsAndQuantityRequired.Count == this.listOfElementsAndQuantityRequired.Count)
            {
                bool flag = true;

                for (int k = 0; k < this.listOfElementsAndQuantityRequired.Count; k++)
                {
                    if (!temp.listOfElementsAndQuantityRequired.Contains(this.listOfElementsAndQuantityRequired[k]))
                    {
                        flag = false;
                        break;
                    }
                }

                return flag;
            }
            else
            {
                return false;
            }   

        }
        else
        {
            return false;
        }

    }


}