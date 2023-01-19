using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager
{
    public List<ObjectiveContains> Contient; //remplissage
    public List<ObjectivePlaceItem> Put; //placer
    public List<ObjectiveGrabItem> take; //prendre

    public ObjectiveManager(List<ObjectiveContains> c, List<ObjectivePlaceItem> p, List<ObjectiveGrabItem> t)
    {
        this.Contient = c;
        this.Put = p;
        this.take = t;
    }
}
