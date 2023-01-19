using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ObjectiveGrabItem : Objective
{
    //Tag de l'objet que l'on veut
    //On pourra modifier plus tard pour un systeme plus complexe
    public string container;
    public int numero;

    //Constructeur
    public ObjectiveGrabItem(string tagOrName, int nb)
    {
        this.container = tagOrName;
        this.numero = nb;
    }

    public override bool Evaluate(Objective obj)
    {
        
        if (obj.GetType() == typeof(ObjectiveGrabItem))
        {
            ObjectiveGrabItem temp = (ObjectiveGrabItem)obj;
            return temp.container.Equals(this.container);
        }
        else
        {
            return false;
        }
    }


}