using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObjectSorbonne : MonoBehaviour
{
    //*********************************************************** VARIABLES

    public delegate void PositionSet(int levelOfSecurity);
    public static event PositionSet OnPositionSet;

    public GameObject levelManager;
    bool isInSorbonne;

    //*********************************************************** FONCTIONS

    void OnEnable()
    {
        isInSorbonne = levelManager.GetComponent<LevelOneLevelManager>().isInSorbonne;
        transform.position = new Vector3(8.08f, 1f, -4.70f);
        //print(transform.position);
    }

    private Vector3 GetMousePosition() //position souris dans le jeu
    {
        Vector3 mousePosition = Input.mousePosition; //position of mouse in pixels
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z; //test(objet) position in pixels
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition); //convertion to position in game 

        return mousePosition;
    }

    private void OnMouseDrag() //drag
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!isInSorbonne && Physics.Raycast(ray, out hit) && hit.transform.name.Equals("Vitre")) //si vitre
        {
            Vector3 mousePosition = GetMousePosition();
            transform.position = new Vector3(transform.position.x, mousePosition.y, transform.position.z);

            //attention pas aller trop loin
            if (transform.position.y < 0.99f)
            {
                transform.position = new Vector3(transform.position.x, 0.99f, transform.position.z);
            }
            else if (transform.position.y > 1.83f)
            {
                transform.position = new Vector3(transform.position.x, 1.83f, transform.position.z);
            }

        }
    }

    private void OnMouseUp() //lacher
    {
        float wantedPosition = 1.44f; //position souhaitée
        if (transform.position.y <= wantedPosition+0.1) //si bien placé
        {
            if (OnPositionSet != null)
            {
                OnPositionSet(3); //message ok (niveau 3 placeholder)
            } 
        }
        else //si mal placé
        {
            if (OnPositionSet != null)
            {
                OnPositionSet(1); //(niveau 1 placeholder)
            }
        }
    }
}
