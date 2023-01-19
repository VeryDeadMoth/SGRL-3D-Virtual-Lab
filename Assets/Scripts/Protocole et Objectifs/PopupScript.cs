using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PopupScript : MonoBehaviour
{
    public delegate void PopupEvents();
    public static event PopupEvents OnPopupOpensEvent;
    public static event PopupEvents OnPopupClosesEvent;

    public TMP_Text popupText;
    public GameObject backgroundImage;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
        backgroundImage.SetActive(false);

        //subscribes au protocole
        Protocole.OnErrorDetectedEvent += Open;
    }

    //Apparition du popup
    public void Open(string message)
    {
        Debug.Log(message);
        /*backgroundImage.SetActive(true);
        popupText.text = message;
        transform.LeanScale(new Vector3(9.2f, 1.34f, 1), 0.5f);
        OnPopupOpensEvent();*/
    }

    //fermeture du popup
    public void Close()
    {
        backgroundImage.SetActive(false);
        transform.LeanScale(Vector3.zero, 0.5f).setEaseInBack();
        OnPopupClosesEvent();
    }
}