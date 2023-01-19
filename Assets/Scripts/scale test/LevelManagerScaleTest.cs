using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerScaleTest : MonoBehaviour
{

    //************************************************************* VARIABLES

    // gants :
    bool glovesOn = false;
    bool glovesAreUnclean = false;

    // objet tenu :
    bool isHolding = false;
    GameObject objectHeld = null;

    // zone / vue
    string area = "paillasse"; // -> change json as it is stated as "paillaisse"

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

    //scale open
    public bool isScaleOpen = false;

    public GameObject scalePlaceholder;

    public bool isScaleBroken = false;

    public GameObject scaleDoorUp;

    //************************************************************* FONCTIONS

    void Start()
    {
        //Call json
        allPossibleErrors = this.protocole.DeserializeJSONErrors(jsonErrorFile);
    }

    //MAIN
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && mouseEnabled) //clique gauche
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) //si contact avec un objet
            {
                GameObject target = hit.transform.gameObject;

                if (!isHolding && (target.CompareTag("container") || target.CompareTag("holder"))) //si main vide et target est un container
                {
                    HoldObject(target);
                }
                else if (isHolding && target.CompareTag("placeholder") && objectHeld.CompareTag("container")) // si main non vide et target est un placeholder
                {
                    PlaceObject(target);
                }
                else if (isHolding && target.CompareTag("container")) //si main non vide et target est un container 
                {
                    if (!target.Equals(objectHeld)) //si target n'est pas l'objet tenu
                    {
                        //check fill errors before filling -> prevents you from filling if error detected (within fill container)
                        FillContainer(target);
                    }
                    else //si target est l'objet tenu
                    {
                        //mix ?
                    }

                }
                else if (isHolding && target.CompareTag("unmovable_holder") && objectHeld.CompareTag("holder")) //tool sur unmovable holder 
                {
                    FillHolder(target);
                }
                else if (target.CompareTag("scale")) //si target est poignet scale
                {
                    isScaleOpen = !isScaleOpen;
                    OnScaleInteraction();
                }

            }
            else //pas de contact
            {
                if (isHolding && objectHeld.CompareTag("holder") && !objectHeld.GetComponent<HoldingTool>().isFull)
                {
                    ReturnTool();
                }
            }
        }
        
    }

    //OTHER

    void HoldObject(GameObject target) // target is the object to hold
    {
        //check hold errors
        

        //placeholder needs to appear - for containers only here
        if (target.CompareTag("container"))
        {
            GameObject tempHiddenPlaceholder = target.GetComponent<ContainerObjectScript>().hiddenPlaceholder;


            if (tempHiddenPlaceholder.name.Equals("Scale") && isScaleOpen) //si placeholder de la balance
            {
                tempHiddenPlaceholder.GetComponent<Placeholderscripttest>().scaleText.text = "0,00g";

                tempHiddenPlaceholder.SetActive(true);
                tempHiddenPlaceholder.GetComponent<Placeholderscripttest>().occupyingObject = null;

                this.isHolding = true;
                this.objectHeld = target;

                target.LeanMove(handPlacement.transform.position, 0.5f).setEaseOutQuart();
                StartCoroutine(TimeUntilMouseEnables(0.5f)); //animation time 
            }
            else if (!tempHiddenPlaceholder.name.Equals("Scale"))
            {

                tempHiddenPlaceholder.SetActive(true);
                tempHiddenPlaceholder.GetComponent<Placeholderscripttest>().occupyingObject = null;

                this.isHolding = true;
                this.objectHeld = target;

                target.LeanMove(handPlacement.transform.position, 0.5f).setEaseOutQuart();
                StartCoroutine(TimeUntilMouseEnables(0.5f)); //animation time 
            }

        }
        else //if not a container
        {
            this.isHolding = true;
            this.objectHeld = target;

            target.LeanMove(handPlacement.transform.position, 0.5f).setEaseOutQuart();
            StartCoroutine(TimeUntilMouseEnables(0.5f)); //animation time 
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
            StartCoroutine(TimeUntilMouseEnables(0.5f)); //animation time 

            if (target.name.Equals("Scale") && !isScaleBroken) //si placeholder de la balance
            {
                float fakeWeight = objectHeld.GetComponent<ContainerObjectScript>().weight + Random.Range(1f,5f);
                target.GetComponent<Placeholderscripttest>().scaleText.text = string.Format("{0:0.00}g", fakeWeight); //affiche quelque chose de faux
            }

            this.objectHeld = null;

            //placeholder will dissappear
            target.SetActive(false);
        }
        
    }

    void ReturnTool() 
    {
        this.isHolding = false;

        objectHeld.LeanMove(objectHeld.GetComponent<HoldingTool>().originalPlacement, 0.5f).setEaseOutQuart();
        StartCoroutine(TimeUntilMouseEnables(0.5f)); //animation time

        this.objectHeld = null;
    }

    void FillContainer(GameObject target) // target is the container (prelevement ici aussi)
    {

        Vector3 tempPosition = target.transform.position;
        tempPosition.y += 0.05f;

        ContainerObjectScript targetScript = target.GetComponent<ContainerObjectScript>();

        if (objectHeld.CompareTag("container")) //si on verse avec container
        {
            //check fill errors
            foreach (KeyValuePair<string, float> pair in objectHeld.GetComponent<ContainerObjectScript>().elementsContained)
            {
                //targetScript.FillObject(pair.Key,pair.Value,weightGoal);
            }

            objectHeld.LeanMove(tempPosition, 0.4f).setEaseOutQuart().setLoopPingPong(1);
            StartCoroutine(TimeUntilMouseEnables(0.8f)); //animation time

        }
        else if (objectHeld.CompareTag("holder")) //si on verse avec holder
        {
            HoldingTool holdingScript = objectHeld.GetComponent<HoldingTool>();
            if (holdingScript.isFull && ((targetScript.hiddenPlaceholder.name.Equals("Scale")&&isScaleOpen)||!targetScript.hiddenPlaceholder.name.Equals("Scale")))
            {
                //check fill errors
                //********************************************************************************************************** WORKING HERE
                if (targetScript.elementsContained.Count >= 1) //Si container a 1 ou plus éléments
                {
                    foreach (KeyValuePair<string, float> pair in targetScript.elementsContained)
                    {
                        ErrorFilling error = new ErrorFilling("", targetScript.containerName, pair.Key, holdingScript.containsName, targetScript.danger, targetScript.hiddenPlaceholder.GetComponent<Placeholderscripttest>().place, targetScript.fill, targetScript.wasMixed);

                        bool results = protocole.CheckFillErrors(error, allPossibleErrors.fill);
                        print(results);

                        if (results && targetScript.hiddenPlaceholder.name.Equals("Scale")) //si erreur sur scale
                        {
                            isScaleBroken = true;
                            targetScript.hiddenPlaceholder.GetComponent<Placeholderscripttest>().scaleText.text = "0,00g";
                        }

                    }
                }
                else //sinon, si container a 0 élément
                {
                    ErrorFilling error = new ErrorFilling("", targetScript.containerName, null, holdingScript.containsName, targetScript.danger, targetScript.hiddenPlaceholder.GetComponent<Placeholderscripttest>().place, targetScript.fill, targetScript.wasMixed);

                    bool results = protocole.CheckFillErrors(error, allPossibleErrors.fill);
                    print(results);
                    
                    if (results && targetScript.hiddenPlaceholder.name.Equals("Scale")) //si erreur sur scale
                    {
                        isScaleBroken = true;
                        targetScript.hiddenPlaceholder.GetComponent<Placeholderscripttest>().scaleText.text = "0,00g";
                    }
                    
                }


                //targetScript.FillObject(holdingScript.containsName, holdingScript.containsQuantity,weightGoal);

                holdingScript.EmptyObject();

                objectHeld.LeanMove(tempPosition, 0.5f).setEaseOutQuart();
                StartCoroutine(TimeUntilMouseEnables(0.5f)); //animation time
                LeanTween.delayedCall(0.5f, ReturnTool);
            }
            else //si holding tool vide, prelevement
            {
                //check prelevement errors

                if (targetScript.weight > 0) //prelevement seulement si poids pas nul
                {
                    //targetScript.TakeFromObject(0f, weightGoal);

                    holdingScript.FillObject("", 0);
                }
                
                objectHeld.LeanMove(tempPosition, 0.4f).setEaseOutQuart().setLoopPingPong(1);
                StartCoroutine(TimeUntilMouseEnables(0.8f)); //animation time
            }
            
        }
    }

    void FillHolder(GameObject target) //target is unmovable holder
    {
        if (objectHeld.GetComponent<HoldingTool>().isFull)
        {
            objectHeld.GetComponent<HoldingTool>().EmptyObject();

            Vector3 tempPosition = target.transform.position;
            tempPosition.y += 0.1f;

            objectHeld.LeanMove(tempPosition, 0.5f).setEaseOutQuart();
            StartCoroutine(TimeUntilMouseEnables(0.5f)); //animation time

            LeanTween.delayedCall(0.5f, ReturnTool);
        }
        else
        {
            //check erreurs prelevement

            Vector3 tempPosition = target.transform.position;
            tempPosition.y += 0.1f;

            objectHeld.LeanMove(tempPosition, 0.4f).setEaseOutQuart().setLoopPingPong(1);
            
            StartCoroutine(TimeUntilMouseEnables(0.8f)); //animation time

            objectHeld.GetComponent<HoldingTool>().FillObject(target.GetComponent<HoldingTool>().containsName, target.GetComponent<HoldingTool>().containsQuantity);
        }
    }

    void OnScaleInteraction() //interaction avec la balance -> changement statut placeholder 
    {
        Placeholderscripttest tempScalePlaceholder = scalePlaceholder.GetComponent<Placeholderscripttest>();
        tempScalePlaceholder.isReachable = isScaleOpen;

        if (!isScaleOpen && tempScalePlaceholder.occupyingObject!=null && !isScaleBroken)
        {
            tempScalePlaceholder.scaleText.text = string.Format("{0:0.00}g", tempScalePlaceholder.occupyingObject.GetComponent<ContainerObjectScript>().weight);
        }

        if (!isScaleOpen) //si elle etait ouverte mais on va fermer
        {
            scaleDoorUp.LeanMoveLocalY(-0.055f, 0.3f);
        }
        else //si elle etait fermée mais on va ouvrir
        {
            scaleDoorUp.LeanMoveLocalY(0.055f, 0.3f);
        }

    }

    public IEnumerator TimeUntilMouseEnables(float seconds)
    {
        mouseEnabled = false;
        yield return new WaitForSeconds(seconds);
        mouseEnabled = true;
    }

}
 