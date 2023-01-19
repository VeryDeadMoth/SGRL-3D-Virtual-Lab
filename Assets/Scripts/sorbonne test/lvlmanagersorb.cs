using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lvlmanagersorb : MonoBehaviour
{
    //************************************************************* VARIABLES

    //position pour camera en mode sorbonne proche -> x0 y9.69 z-14.6 angle x17.479

    // gants :
    bool glovesOn = false;
    bool glovesAreUnclean = false;

    // objet tenu :
    bool isHolding = true;
    public GameObject objectHeld;

    // zone / vue
    string area = "sorbonne";

    //player
    bool mouseEnabled = true;

    //hand
    public GameObject handPlacement;

    //poids (temp)
    public float weightGoal;

    //protocole
    private Protocole protocole = new Protocole();


    //JSON
    public TextAsset jsonErrorFile;

    ErrorManager allPossibleErrors; //erreurs

    //sorbonne
    //si en zone sorbonne (variable area), check :
    public bool isInSorbonne = false;

    public List<GameObject> sorbonnePlaceholders;

    //camera
    public GameObject myCamera;

    //vitre sorbonne
    public GameObject sorbonneWindow;
    public GameObject sorbonne;

    //scale open
    /*public bool isScaleOpen = false;

    public GameObject scalePlaceholder;

    public bool isScaleBroken = false;

    public GameObject scaleDoorUp;*/

    //************************************************************* FONCTIONS

    // Start is called before the first frame update
    void Start()
    {
        //Call json
        allPossibleErrors = this.protocole.DeserializeJSONErrors(jsonErrorFile);

        DragObjectSorbonne.OnPositionSet += SetPlaceholderLevel;

        foreach (GameObject obj in sorbonnePlaceholders)
        {
            obj.SetActive(false);
            Placeholderscripttest temp = obj.GetComponent<Placeholderscripttest>();
            temp.isReachable = false;
            temp.place = 1;
        }

        sorbonneWindow.GetComponent<DragObjectSorbonne>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInSorbonne) //si en vue sorbonne
        {
            if (Input.GetMouseButtonDown(0) && mouseEnabled) //si clique gauche
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit)) //si contact avec un objet
                {
                    GameObject target = hit.transform.gameObject;

                    if (isHolding && target.CompareTag("placeholder") && objectHeld.CompareTag("container")) // si main non vide et target est un placeholder
                    {
                        PlaceObject(target);
                    }
                }
                
            }
            else if(Input.GetMouseButtonDown(1) && mouseEnabled)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit)) //si contact avec un objet
                {
                    GameObject target = hit.transform.gameObject;

                    if (!isHolding && target.CompareTag("container")) // si main non vide et target est un placeholder
                    {
                        target.GetComponent<ContainerObjectScript>().capIsOn = !target.GetComponent<ContainerObjectScript>().capIsOn;

                        if (!target.GetComponent<ContainerObjectScript>().capIsOn)
                        {
                            //erreurs bouchon
                            ErrorLid error = new ErrorLid("", target.GetComponent<ContainerObjectScript>().danger, target.GetComponent<ContainerObjectScript>().hiddenPlaceholder.GetComponent<Placeholderscripttest>().place);
                            print(protocole.CheckLidErrors(error, allPossibleErrors.takeOffCap));
                        }
                        
                    }
                }
            }
        }
        else //si pas en vue sorbonne
        {
            if (Input.GetMouseButtonDown(0) && mouseEnabled)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit)) //si contact avec un objet
                {
                    GameObject target = hit.transform.gameObject;

                    if (target.name.Equals("Sorbonne"))
                    {
                        isInSorbonne = true;
                        Vector3 temp = new Vector3(0,9.69f,-14.6f);
                        myCamera.LeanMove(temp, 0.5f);
                        myCamera.LeanRotateX(17.479f, 0.5f); //movement camera

                        ActivatePlaceholder(true); //placeholder visible
                        sorbonneWindow.GetComponent<BoxCollider>().enabled = false;

                        sorbonne.GetComponent<BoxCollider>().enabled = false; 
                    }
                }
            }
        }
    }

    void SetPlaceholderLevel(int level)
    {
        foreach (GameObject obj in sorbonnePlaceholders)
        {
            Placeholderscripttest temp = obj.GetComponent<Placeholderscripttest>();
            temp.place = level;
        }
    }

    void ActivatePlaceholder(bool activated)
    {
        foreach (GameObject obj in sorbonnePlaceholders)
        {
            obj.SetActive(activated);
            Placeholderscripttest temp = obj.GetComponent<Placeholderscripttest>(); //optionnel
            temp.isReachable = true; 
        }
    }

    void PlaceObject(GameObject target) // target is the placeholder selected
    {
        //check put errors

        if (target.GetComponent<Placeholderscripttest>().isReachable)
        {
            this.isHolding = false;

            this.objectHeld.GetComponent<ContainerObjectScript>().hiddenPlaceholder = target; // for containers only
            target.GetComponent<Placeholderscripttest>().occupyingObject = this.objectHeld;

            this.objectHeld.LeanMove(target.transform.position, 0.5f).setEaseOutQuart();
            mouseEnabled = false;
            LeanTween.delayedCall(0.5f, EnableMouse);

            /*if (target.name.Equals("Scale") && !isScaleBroken) //si placeholder de la balance
            {
                float fakeWeight = objectHeld.GetComponent<ContainerObjectScript>().weight + Random.Range(1f, 5f);
                target.GetComponent<Placeholderscripttest>().scaleText.text = string.Format("{0:0.00}g", fakeWeight); //affiche quelque chose de faux
            }*/

            this.objectHeld = null;

            //placeholder will dissappear
            target.SetActive(false);
        }

    }

    void EnableMouse()
    {
        mouseEnabled = true;
    }

}
