using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    public string message;

    private void Start()
    {
        transform.GetComponent<Outline>().enabled = false;
    }
    private void OnMouseEnter()
    {
        ToolTipManager._instance.SetAndShowToolTip(message);

        transform.GetComponent<Outline>().enabled = true;
    }

    private void OnMouseExit()
    {
        ToolTipManager._instance.HideToolTip();

        transform.GetComponent<Outline>().enabled = false;
    }
}
