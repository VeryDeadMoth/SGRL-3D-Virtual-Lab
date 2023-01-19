using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Container : MonoBehaviour
{

    public abstract void ObjectWasFilled();
    public abstract void ObjectGotGrabbed();
    public abstract void AddElement(Dictionary<string, int> elementDictionary);
    public abstract void SelfFill();
    public abstract void ClearObject();

}
