using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FullProtocole_Test_Scale : MonoBehaviour
{
    //CHANGE TO FLOATS
    public TMP_Text scaleDisplay;
    public void WeighObject(Dictionary<string, int> dictionaryOfElements)
    {
        int total = 0;
        foreach (KeyValuePair<string, int> pair in dictionaryOfElements)
        {
            total += pair.Value;
        }

        scaleDisplay.text = total + " g";
    }
}
