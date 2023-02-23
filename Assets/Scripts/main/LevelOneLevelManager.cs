using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelOneLevelManager : MonoBehaviour
{
    //*********************************************************** VARIABLES

    // gants :
    bool glovesOn = false;
    bool glovesAreUnclean = false;

    // objet tenu :
    public bool isHolding = false;

    [SerializeField]
    public GameObject objectHeld = null;

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
    public TextAsset jsonObjectiveFile;

    ErrorManager allPossibleErrors; //erreurs


    //scale open
    public bool isScaleOpen = false;

    public GameObject scalePlaceholder;

    public bool isScaleBroken = false;

    public GameObject scaleDoorUp;

    //Variables pour sorbonne 
    public bool isInSorbonne = false; // "en vue sorbonne ?"
    public List<GameObject> sorbonnePlaceholders;

    //Liste des toggles (ordonnés en fonction des objectifs)
    public List<GameObject> toggleList1;
    public List<GameObject> toggleList2;

    //await filling input -> si true, le joueur est en phase de remplissage (longue et non directe comme la cuilliere)
    public bool isAwaitingFillInput = false;
    GameObject targetForInput;

    // jauge de content
    public GameObject sliderImage;

    
    
    [Header("Mix Part")]//mix
    public GameObject rotation;
    public GameObject mixButton;
    public GameObject cancelMixCube;
    public GameObject pluMin;

    Quaternion baseRot;

    //*********************************************************** FONCTIONS

    private void Awake() //inscription aux events
    {
        CameraManager.OnNewArea += SetArea; //changement de zone
        ContainerObjectScript.ObjectHadSomethingHappenEvent += this.protocole.checkIfOrderedObjectiveIsValidated; 
        HoldingTool.ObjectHadSomethingHappenEvent += this.protocole.checkIfOrderedObjectiveIsValidated;
        Protocole.OnObjectiveSuccessfullyCompletedEvent += ToggleUpdate;
    }

    //MAIN
    
    void Start()
    {
        //Call json
        //allPossibleErrors = this.protocole.DeserializeJSONErrors(jsonErrorFile);
        protocole.DeserializeJSONProtocole(jsonObjectiveFile);
        //set gauge à 0 
        sliderImage.GetComponent<Image>().fillAmount = 1;
    }

    void Update()
    {
        /*if(objectHeld)
        {
            objectHeld.transform.position = handPlacement.transform.position;
            objectHeld.transform.rotation = new Quaternion(objectHeld.transform.rotation.x, -handPlacement.transform.rotation.y, objectHeld.transform.rotation.z, objectHeld.transform.rotation.w);
        }*/
        if (Input.GetMouseButtonDown(0) && mouseEnabled) //clique gauche
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) //si contact avec un objet
            {
                GameObject target = hit.transform.gameObject;

                if (isAwaitingFillInput) //en attente input
                {
                    if (target.Equals(objectHeld) && (target.CompareTag("holder")|| target.CompareTag("pissette"))) //si je clique sur mon holder 
                    {
                        FillContainer(targetForInput);
                    }
                }
                else
                {

                    if (!isHolding && (target.CompareTag("container") || target.CompareTag("holder") || target.CompareTag("funnel") || target.CompareTag("pissette"))) //si main vide et target est un container/funnel/holder/pissette
                    {
                        HoldObject(target);
                    }
                    else if (isHolding && target.CompareTag("placeholder") && objectHeld.CompareTag("container")) // si main non vide et target est un placeholder et je tiens un container
                    {
                        PlaceObject(target);
                    }
                    else if (isHolding && target.CompareTag("Bin") && objectHeld.CompareTag("container")) // si main non vide et target est la poubelle et je tiens un container
                    {
                        Vector3 temp = new Vector3(-0.1f,0.35f,0);
                        objectHeld.GetComponent<ContainerObjectScript>().EmptyObject(); //je vide mon container
                        objectHeld.LeanMove(target.transform.position + temp, 0.4f).setEaseOutQuart().setLoopPingPong(1);
                    }
                    else if (isHolding && target.CompareTag("container")) //si main non vide et target est un container 
                    {
                        if (!target.Equals(objectHeld)) //si target n'est pas l'objet tenu
                        {
                            //check fill errors before filling -> prevents you from filling if error detected (within fill container)
                            if (objectHeld.CompareTag("funnel") && target.name.Contains("Fiole")) //si je tiens funnel et container est fiole
                            {

                            }
                            /*else if (objectHeld.CompareTag("pissette")) //si l'objet tenu est une pissette
                            {
                                isAwaitingFillInput = true;
                                //MOUVEMEMENT VERS CONTAINER
                            }*/
                            else //sinon
                            {
                                FillContainer(target);
                            }

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
                    else if (target.CompareTag("cancelMix")) //si target est le mur a coté
                    {
                        DisableMix();
                    }
                }

            }
            else //pas de contact
            {
                if (isHolding && ((objectHeld.CompareTag("holder") && !objectHeld.GetComponent<HoldingTool>().isFull) || (objectHeld.CompareTag("funnel") || objectHeld.CompareTag("pissette"))))
                {
                    ReturnTool();
                }
            }
        }
        else if (Input.GetMouseButtonDown(1) && mouseEnabled)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) //si contact avec un objet
            {
                GameObject target = hit.transform.gameObject;
                if (target.CompareTag("container"))
                {
                    //check erreurs ?
                    target.GetComponent<ContainerObjectScript>().CapInteraction();
                }
            }
        }
        //si la jauge tombe a zéro on active le script de nettoyage 
        if (sliderImage.GetComponent<Image>().fillAmount <= 0.1)
        {
            GetComponent<CleanManager>().enabled = true;
        }
    }

    //AUTRE

    void EnableMouse()
    {
        mouseEnabled = true;
    }

    void SetArea(int intArea) //0 = paillasse / 1 = balance / 2 = sorbonne
    {
        if (intArea == 0)
        {
            area = "paillasse";
        }
        else if (intArea == 1)
        {
            area = "scale";
        }
        else
        {
            area = "sorbonne";
        }
        
    }

    void ToggleUpdate()
    {
        toggleList1[this.protocole.objectivesCounter].GetComponent<Toggle>().isOn = true;
        toggleList2[this.protocole.objectivesCounter].GetComponent<Toggle>().isOn = true;
    }

    void OnScaleInteraction() //interaction avec la balance -> changement statut placeholder  (//****************************************************************** CHANGER POSITIONS)
    //upon closing or opening the scale, if an object is placed on it, the object's collider will be deactivated or re-activated (prevents people from filling object if scale closed)
    {
        Placeholderscripttest tempScalePlaceholder = scalePlaceholder.GetComponent<Placeholderscripttest>();
        tempScalePlaceholder.isReachable = isScaleOpen;

        if (!isScaleOpen && tempScalePlaceholder.occupyingObject != null && !isScaleBroken)
        {
            tempScalePlaceholder.scaleText.text = string.Format("{0:0.00}g", tempScalePlaceholder.occupyingObject.GetComponent<ContainerObjectScript>().weight);
        }

        if (!isScaleOpen) //si elle etait ouverte mais on va fermer
        {
            scaleDoorUp.LeanMoveLocalY(-0.055f, 0.3f);
            //if has an object
            if (tempScalePlaceholder.occupyingObject!= null)
            {
                tempScalePlaceholder.occupyingObject.GetComponent<BoxCollider>().enabled = false;
            }
        }
        else //si elle etait fermée mais on va ouvrir
        {
            scaleDoorUp.LeanMoveLocalY(0.055f, 0.3f);
            //if has an object
            if (tempScalePlaceholder.occupyingObject != null)
            {
                tempScalePlaceholder.occupyingObject.GetComponent<BoxCollider>().enabled = true;
            }
        }

    }

    public void EnableMix() //permet de melanger le contenu d'une fiole
    {
        pluMin.SetActive(false);
        mixButton.SetActive(false);
        cancelMixCube.SetActive(true);
        rotation.GetComponent<SpinSquare>().fiole = objectHeld;
        rotation.SetActive(true);
        
        Camera.main.GetComponent<CameraManager>().GoToMix();

        
    }

    public void DisableMix()
    {
        pluMin.SetActive(true);
        mixButton.SetActive(true);
        cancelMixCube.SetActive(false);
        Camera.main.GetComponent<CameraManager>().GoBackMix();
        rotation.SetActive(false);
    }

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
            }

            if (!tempHiddenPlaceholder.name.Equals("Scale") || isScaleOpen)
            {
                tempHiddenPlaceholder.SetActive(true);
                tempHiddenPlaceholder.GetComponent<Placeholderscripttest>().occupyingObject = null;

                this.isHolding = true;
                this.objectHeld = target;
                baseRot = objectHeld.transform.rotation;
                objectHeld.transform.SetParent(handPlacement.transform);

                target.GetComponent<ContainerObjectScript>().GrabObject();

                if (target.name.Contains("Fiole")) //si objet est une fiole
                {


                    Vector3 tempPosition = new Vector3(handPlacement.transform.position.x, handPlacement.transform.position.y + 0.2f, handPlacement.transform.position.z);
                    target.LeanMove(tempPosition, 0.5f).setEaseOutQuart();

                    mouseEnabled = false;
                    LeanTween.delayedCall(0.5f, EnableMouse);

                    //EnableMix(); //melange
                    mixButton.SetActive(true);
                }
                else
                {
                    

                    target.LeanMove(handPlacement.transform.position, 0.5f).setEaseOutQuart();

                    mouseEnabled = false;
                    LeanTween.delayedCall(0.5f, EnableMouse);
                }
            }
            

        }
        else if (target.CompareTag("pipette"))
        {
            target.GetComponent<ContainerObjectScript>().GrabObject();

            Vector3 temp = new Vector3(handPlacement.transform.position.x, handPlacement.transform.position.y + 2f, handPlacement.transform.position.z);
            target.LeanMove(temp, 0.5f).setEaseOutQuart();
        }
        else if(target.CompareTag("holder")) //if not a container
        {
            this.isHolding = true;
            this.objectHeld = target;
            baseRot = objectHeld.transform.rotation;
            objectHeld.transform.SetParent(handPlacement.transform);

            target.GetComponent<HoldingTool>().GrabObject(); //HOLDER

            target.LeanMove(handPlacement.transform.position, 0.5f).setEaseOutQuart();

            mouseEnabled = false;
            LeanTween.delayedCall(0.5f, EnableMouse);
        }
        else
        {
            this.isHolding = true;
            this.objectHeld = target;
            baseRot = objectHeld.transform.rotation;
            objectHeld.transform.SetParent(handPlacement.transform);

            target.LeanMove(handPlacement.transform.position, 0.5f).setEaseOutQuart();

            mouseEnabled = false;
            LeanTween.delayedCall(0.5f, EnableMouse);
        }
    }

    void PlaceObject(GameObject target) // target is the placeholder selected (container only for now)
    {
        //check put errors

        if (target.GetComponent<Placeholderscripttest>().isReachable && ((target.name.Equals("Placeholder Tube")&&objectHeld.name.Contains("Tube_Essai")) || !target.name.Equals("Placeholder Tube") && !objectHeld.name.Contains("Tube_Essai")))
        {
            //DisableMix();
            mixButton.SetActive(false);

            this.isHolding = false;

            this.objectHeld.GetComponent<ContainerObjectScript>().hiddenPlaceholder = target; // for containers only
            target.GetComponent<Placeholderscripttest>().occupyingObject = this.objectHeld;

            objectHeld.GetComponent<ContainerObjectScript>().DropObject(area,target.GetComponent<Placeholderscripttest>().place);

            if (objectHeld.name.Contains("Fiole"))
            {
                Vector3 tempPosition = new Vector3(target.transform.position.x,target.transform.position.y+ objectHeld.GetComponent<Collider>().bounds.size.y, target.transform.position.z);
                this.objectHeld.LeanMove(tempPosition, 0.5f).setEaseOutQuart();
            }
            else
            {
                this.objectHeld.LeanMove(target.transform.position, 0.5f).setEaseOutQuart();
            }

            mouseEnabled = false;
            LeanTween.delayedCall(0.5f, EnableMouse);

            if (target.name.Equals("Scale") && !isScaleBroken) //si placeholder de la balance
            {
                float fakeWeight = objectHeld.GetComponent<ContainerObjectScript>().weight + Random.Range(1f, 5f);
                target.GetComponent<Placeholderscripttest>().scaleText.text = string.Format("{0:0.00}g", fakeWeight); //affiche quelque chose de faux
            }

            objectHeld.transform.SetParent(transform.parent);
            objectHeld.transform.rotation = baseRot;
            this.objectHeld = null;

            //placeholder will dissappear
            target.SetActive(false);
        }

    }

    public void ReturnTool()
    {
        this.isHolding = false;

        this.isAwaitingFillInput = false;
        objectHeld.transform.SetParent(transform.parent);

        if (objectHeld.CompareTag("holder"))
        {
            objectHeld.LeanMove(objectHeld.GetComponent<HoldingTool>().originalPlacement, 0.5f).setEaseOutQuart();
        }
        else if (objectHeld.CompareTag("pissette")) //si pas holder (donc pissette ici)
        {
            objectHeld.LeanMove(objectHeld.GetComponent<StaticHolder>().originalPlacement, 0.5f).setEaseOutQuart();
        }
        
        mouseEnabled = false;
        LeanTween.delayedCall(0.5f, EnableMouse);

        
        objectHeld.transform.rotation = baseRot;
        this.objectHeld = null;

    }

    void FillHolder(GameObject target) //target is unmovable holder
    {
        if (objectHeld.GetComponent<HoldingTool>().isFull) //vider holder
        {
            objectHeld.GetComponent<HoldingTool>().EmptyObject();

            Vector3 tempPosition = target.transform.position;
            tempPosition.y += target.GetComponent<Collider>().bounds.size.y + 0.1f;

            objectHeld.LeanMove(tempPosition, 0.5f).setEaseOutQuart();
            mouseEnabled = false;
            LeanTween.delayedCall(0.5f, EnableMouse);

            LeanTween.delayedCall(0.5f, ReturnTool);
        }
        else //remplir holder
        {
            //check erreurs prelevement

            Vector3 tempPosition = target.transform.position;
            tempPosition.y += target.GetComponent<Collider>().bounds.size.y + 0.1f;

            objectHeld.LeanMove(tempPosition, 0.4f).setEaseOutQuart().setLoopPingPong(1);
            mouseEnabled = false;
            LeanTween.delayedCall(0.8f, EnableMouse);

            objectHeld.GetComponent<HoldingTool>().FillObject(target.GetComponent<HoldingTool>().containsName, target.GetComponent<HoldingTool>().containsQuantity);
        }
    }

    void FillContainer(GameObject target) // target is the container (prelevement ici aussi)
    {

        Vector3 tempPosition = target.transform.position;
        tempPosition.y += target.GetComponent<Collider>().bounds.size.y + 0.02f;
        ContainerObjectScript objectHeldScript = objectHeld.GetComponent<ContainerObjectScript>();
        ContainerObjectScript targetScript = target.GetComponent<ContainerObjectScript>();

        if (objectHeld.CompareTag("container") && (!targetScript.needsCap || (targetScript.needsCap && !targetScript.capIsOn))) //si on verse avec container
        {
            //check fill errors
            foreach (KeyValuePair<string, float> pair in objectHeld.GetComponent<ContainerObjectScript>().elementsContained)
            {
                targetScript.FillObject(pair.Key, pair.Value, objectHeld.GetComponent<ContainerObjectScript>().shaderFill,false);
            }
            targetScript.pH = objectHeldScript.pH;
            targetScript.weight = objectHeldScript.weight;
            objectHeld.GetComponent<ContainerObjectScript>().EmptyObject();

            objectHeld.LeanMove(tempPosition, 0.4f).setEaseOutQuart().setLoopPingPong(1);
            mouseEnabled = false;
            LeanTween.delayedCall(0.8f, EnableMouse);

        }
        else if (objectHeld.CompareTag("holder") && (!targetScript.needsCap || (targetScript.needsCap && !targetScript.capIsOn)) ) //si on verse avec holder
        {
            HoldingTool holdingScript = objectHeld.GetComponent<HoldingTool>();
            if (holdingScript.isFull)
            {
                //check fill errors
                /*foreach (KeyValuePair<string, float> pair in targetScript.elementsContained)
                {
                    ErrorFilling error = new ErrorFilling("", targetScript.containerName, pair.Key, holdingScript.containsName, targetScript.danger, targetScript.hiddenPlaceholder.GetComponent<Placeholderscripttest>().place, targetScript.fill, targetScript.wasMixed);

                    bool results = protocole.CheckFillErrors(error, allPossibleErrors.fill);
                    print(results);

                    if (results && targetScript.hiddenPlaceholder.name.Equals("Scale")) //si erreur sur scale
                    {
                        isScaleBroken = true;
                        targetScript.hiddenPlaceholder.GetComponent<Placeholderscripttest>().scaleText.text = "0,00g";
                    }

                }*/


                if (!objectHeld.name.Equals("Pipette pasteur")) //si pas pipette pasteur
                {
                    targetScript.FillObject(holdingScript.containsName, holdingScript.containsQuantity, holdingScript.fillingValue,true); //AS WEIGHT

                    holdingScript.EmptyObject();
                }
                else //si pipette pasteur
                {
                    
                    targetScript.FillObject(holdingScript.containsName, holdingScript.containsQuantity, 1,false);
                    holdingScript.containsQuantity -= 3.5f;

                    holdingScript.EmptyObject();
                    
                }


                if (!isAwaitingFillInput)
                {
                    objectHeld.LeanMove(tempPosition, 0.5f).setEaseOutQuart();
                    mouseEnabled = false;
                    LeanTween.delayedCall(0.5f, EnableMouse);

                    LeanTween.delayedCall(0.5f, ReturnTool);
                }

            }
            else //si holding tool vide, prelevement
            {
                //check prelevement errors

                if (targetScript.weight > 0) //prelevement seulement si poids pas nul
                {
                    targetScript.TakeFromObject(0f, 0,true);

                    holdingScript.FillObject("", 0);
                }

                objectHeld.LeanMove(tempPosition, 0.4f).setEaseOutQuart().setLoopPingPong(1);
                mouseEnabled = false;
                LeanTween.delayedCall(0.8f, EnableMouse);
            }


        }
        else if (objectHeld.CompareTag("pissette"))
        {
            StaticHolder holdingScript = objectHeld.GetComponent<StaticHolder>();
            targetScript.FillObject(holdingScript.containsName, holdingScript.containsQuantity, holdingScript.fillingValue,false);

            tempPosition = new Vector3(tempPosition.x +0.05f, tempPosition.y- objectHeld.GetComponent<Collider>().bounds.size.y, tempPosition.z);

            objectHeld.LeanMove(tempPosition, 0.4f).setEaseOutQuart().setLoopPingPong(1);
            objectHeld.LeanRotateZ(0, 0.4f).setEaseOutQuart().setLoopPingPong(1);

            mouseEnabled = false; //prevent spam
            LeanTween.delayedCall(0.8f, EnableMouse);

        }
        else if (objectHeld.CompareTag("pipetteA"))
        {
            //check fill errors
            HoldingTool holdingScript = objectHeld.GetComponent<HoldingTool>();

            targetScript.FillObject(holdingScript.containsName, holdingScript.containsQuantity, holdingScript.fillingValue,false);

            holdingScript.EmptyPipette();
        }
    }


}
