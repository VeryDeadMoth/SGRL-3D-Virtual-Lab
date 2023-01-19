using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullProtocole_Test_Level_Manager : MonoBehaviour
{
    //json error file
    public TextAsset jsonErrorFile;

    public AnimationCurve animationCurve;

    //si possibilité interaction
    bool mouseEnabled = true;

    public float animationDuration;

    private Protocole protocole = new Protocole();

    //Liste des toggles (ordonnés en fonction des objectifs)
    public List<GameObject> toggleList1;
    public List<GameObject> toggleList2;


    public GameObject isHolding = null;
    public Vector3 positionBeforeHeld;

    public GameObject myHand;

    //********************TEMPORARY ?
    ErrorManager allPossibleErrors;

    private void OnEnable()
    {
        ContainerSimple.ObjectHadSomethingHappenEvent += this.protocole.checkIfOrderedObjectiveIsValidated;
        HolderSimple.ObjectHadSomethingHappenEvent += this.protocole.checkIfOrderedObjectiveIsValidated;
        Protocole.OnObjectiveSuccessfullyCompletedEvent += ToggleUpdate;
        PopupScript.OnPopupOpensEvent += DisableMouse;
        PopupScript.OnPopupClosesEvent += EnableMouse;

    }

    void ToggleUpdate()
    {
        toggleList1[this.protocole.objectivesCounter].GetComponent<Toggle>().isOn = true;
        toggleList2[this.protocole.objectivesCounter].GetComponent<Toggle>().isOn = true;
    }

    private void Start()
    {
        //this.protocole.listOfObjectives.Add(this.protocole.DeserializeJSONProtocole());
        //this.protocole.dictionaryOfObjectives.Add(this.protocole.DeserializeJSONProtocole(),false);

        this.allPossibleErrors = this.protocole.DeserializeJSONErrors(jsonErrorFile);


        //this.protocole.DeserializeJSONProtocole(); 

        Dictionary<string, int> d1 = new Dictionary<string, int>();
        d1.Add("eau distillée", 100);
        ObjectiveContainsDictionary obj1 = new ObjectiveContainsDictionary(d1, 1);
        this.protocole.listOfObjectives.Add(obj1);
        this.protocole.dictionaryOfObjectives.Add(obj1, false);

        /*//faire une pesee -> test contains strict
        Dictionary<string, int> d2 = new Dictionary<string, int>();
        d2.Add("poudre", 25);
        ObjectiveContainsDictionary obj2 = new ObjectiveContainsDictionary(d2, 1);
        this.protocole.listOfObjectives.Add(obj2);
        this.protocole.dictionaryOfObjectives.Add(obj2, false);

        //test contains strict sur fiole (poudre)
        Dictionary<string, int> d3 = new Dictionary<string, int>();
        d3.Add("eau distillée", 100);
        d3.Add("poudre", 25);
        ObjectiveContainsDictionary obj3 = new ObjectiveContainsDictionary(d3, 1);
        this.protocole.listOfObjectives.Add(obj3);
        this.protocole.dictionaryOfObjectives.Add(obj3, false);

        //test contains strict sur fiole (eau)
        Dictionary<string, int> d4 = new Dictionary<string, int>();
        d4.Add("eau distillée", 150);
        d4.Add("poudre", 25);
        ObjectiveContainsDictionary obj4 = new ObjectiveContainsDictionary(d4, 1);
        this.protocole.listOfObjectives.Add(obj4);
        this.protocole.dictionaryOfObjectives.Add(obj4, false);

        //?
        ObjectiveGrabItem obj5 = new ObjectiveGrabItem("Fiole");
        this.protocole.listOfObjectives.Add(obj5);
        this.protocole.dictionaryOfObjectives.Add(obj5, false);

        //test contains fiole strict (acide)
        Dictionary<string, int> d5 = new Dictionary<string, int>();
        d5.Add("eau distillée", 150);
        d5.Add("poudre", 25);
        d5.Add("acide", 25);
        ObjectiveContainsDictionary obj6 = new ObjectiveContainsDictionary(d5, 1);
        this.protocole.listOfObjectives.Add(obj6);
        this.protocole.dictionaryOfObjectives.Add(obj6, false);

        //
        Dictionary<string, int> d6 = new Dictionary<string, int>();
        d6.Add("eau distillée", 500);
        d6.Add("poudre", 25);
        d6.Add("acide", 25);
        ObjectiveContainsDictionary obj7 = new ObjectiveContainsDictionary(d6, 1);
        this.protocole.listOfObjectives.Add(obj7);
        this.protocole.dictionaryOfObjectives.Add(obj7, false);

        //end protocole*/


    }

    //*******************************************GET RID OF HOLDERS ?
    //drop object will have to be updated when placeholders will be added
    private void Update()
    {
        // ******* GRAB / PUT / DROP - prend un element, verse le contenu de l'objet tenu dans celui cliqué ou lache un element
        if (Input.GetMouseButtonDown(0) && mouseEnabled)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && this.isHolding == null) //rien en main
            {
                GameObject obj = hit.transform.gameObject;
                HoldObject(obj);

            }
            else if (Physics.Raycast(ray, out hit) && this.isHolding != null) //avec qq chose en main
            {
                GameObject obj = hit.transform.gameObject;
                if (obj.name.Equals(isHolding.name)) //clique sur le meme objet
                {
                    DropObject();
                }
                else //pas le meme objet
                {
                    
                    if (this.isHolding.CompareTag("container") && obj.CompareTag("container"))
                    {
                        //IDEAS TO KEEP IN MIND 
                        //**************************** changer par fonction qui prend en charge la creation de toutes les erreurs ??
                        //**************************** code pour verif si container simple ou autre ?
                        //**************************** faire classe container generale (pas container puis container simple et autre) ??
                        if(SetAndCheckFillErrors(this.isHolding, obj))
                        {
                            //************ PREVENTS ACTION FROM BEING DONE IF ERROR
                            obj.GetComponent<Container>().AddElement(this.isHolding.GetComponent<ContainerSimple>().dictionaryOfContainedElements);
                            //print("aaaaaaaaaaaaaaaaaaaaaaaaa");
                        }
                        
                    }
                    else if (this.isHolding.CompareTag("holder") && obj.CompareTag("container"))
                    {
                        SetAndCheckFillErrors(this.isHolding,obj);

                        obj.GetComponent<Container>().AddElement(this.isHolding.GetComponent<HolderSimple>().dictionaryOfContainedElements);
                    }
                    else if (this.isHolding.CompareTag("container") && obj.CompareTag("scale"))
                    {
                        obj.GetComponent<FullProtocole_Test_Scale>().WeighObject(this.isHolding.GetComponent<ContainerSimple>().dictionaryOfContainedElements);
                    }
                }
            }
        }

        // ******* FILL - remplissage d'un element 
        //PENSER A AJOUTER ERREUR ICI AUSSI
        //Note : self fill ne sera pas utilisée dans le version complete
        else if (Input.GetMouseButtonDown(1) && mouseEnabled)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject obj = hit.transform.gameObject;
                if (obj.CompareTag("container"))
                {

                   obj.GetComponent<Container>().SelfFill();
                }
                else if (obj.CompareTag("holder"))
                {
                    obj.GetComponent<HolderSimple>().SelfFill();
                }
            }

        }
        // ******* CLEAR - vide le contenu de l'element
        else if (Input.GetMouseButtonDown(2) && mouseEnabled)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject obj = hit.transform.gameObject;
                if (obj.CompareTag("container"))
                {
                    obj.GetComponent<Container>().ClearObject();
                }
                else if (obj.CompareTag("holder"))
                {
                    obj.GetComponent<HolderSimple>().ClearObject();
                }
            }
        }
    }

    //saisir un objet
    void HoldObject(GameObject gObj)
    {
        StopAllCoroutines();
        if (gObj.CompareTag("container"))
        {
            this.isHolding = gObj;
            this.positionBeforeHeld = gObj.transform.position;
            StartCoroutine(SmoothPos(gObj, gObj.transform.position, myHand.transform.position));
            gObj.transform.position = myHand.transform.position;
            gObj.GetComponent<Container>().ObjectGotGrabbed();
        }
        else if (gObj.CompareTag("holder"))
        {
            this.isHolding = gObj;
            this.positionBeforeHeld = gObj.transform.position;
            StartCoroutine(SmoothPos(gObj, gObj.transform.position, myHand.transform.position));
            gObj.transform.position = myHand.transform.position;
            gObj.GetComponent<HolderSimple>().ObjectGotGrabbed();
        }
    }

    //lacher un objet (retourne à sa position precedente)
    void DropObject()
    {
        StopAllCoroutines();
        StartCoroutine(SmoothPos(this.isHolding, this.isHolding.transform.position, positionBeforeHeld));
        this.isHolding.transform.position = positionBeforeHeld;
        this.isHolding = null;

    }

    void EnableMouse()
    {
        mouseEnabled = true;
    }

    void DisableMouse()
    {
        mouseEnabled = false;
    }

    //creation des erreurs à verifier + regarde si erreurs ou non
    //AJOUTER AUTRES PARAMETRES SI NECESSAIRE
    bool SetAndCheckFillErrors(GameObject pouredIn, GameObject receiving)
    {
        Dictionary<string, int> pouredDico = new Dictionary<string, int>();
        ErrorFilling error;

        if (pouredIn.CompareTag("holder"))
        {
            pouredDico = pouredIn.GetComponent<HolderSimple>().dictionaryOfContainedElements;
        }
        else if (pouredIn.CompareTag("container"))
        {
            pouredDico = pouredIn.GetComponent<ContainerSimple>().dictionaryOfContainedElements;
        }
        //change receiving.name by proper varaiable if names of objects in game arent correct
        //two loops -> compare all elements of both what pours and what receives -> (there wont be extra elements in poured in- only one generally)
        
        foreach(KeyValuePair<string, int> pair in pouredDico)
        {
            foreach(KeyValuePair<string, int> pair2 in receiving.GetComponent<ContainerSimple>().dictionaryOfContainedElements)
            {
                //first is what is already in, then what is added
                error = new ErrorFilling(null, receiving.name,pair2.Key,pair.Key,0,1,10,false);
                if(protocole.checkIfErrorWasDoneFill(error, allPossibleErrors.fill, receiving.GetComponent<ContainerSimple>().dictionaryOfContainedElements.Count))
                {
                    return false;
                }
                
            }
        }
        return true;
        //ErrorWaterInAcid er1 = new ErrorWaterInAcid(pouredIn,alreadyIn);
        
    }

    //add loop to set all toggle labels from text file + set all toggles to isOn= false + interactable off

    //******************************************************************************************************
    //UPDATE WITH LEAN TWEEN ???

    public IEnumerator SmoothPos(GameObject targetToMove, Vector3 a, Vector3 b) //animation de deplacement base
    {
        mouseEnabled = false;
        float startTime = Time.realtimeSinceStartup;
        float currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / animationDuration) : (1.0f);
        while (currentTimePercentage <= 1.0f)
        {
            targetToMove.transform.position = Vector3.Lerp(a, b, animationCurve.Evaluate(currentTimePercentage));
            yield return null;
            currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / animationDuration) : (1.0f);
        }
        mouseEnabled = true;
    }
    public IEnumerator SmoothRotX(GameObject targetToMove, float a, float b) //animation de rotation base
    {
        mouseEnabled = false;
        float startTime = Time.realtimeSinceStartup;
        float currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / (animationDuration)) : (1.0f);
        while (currentTimePercentage <= 1.0f)
        {
            targetToMove.transform.rotation = new Quaternion(Mathf.Lerp(a, b, animationCurve.Evaluate(currentTimePercentage)), targetToMove.transform.rotation.y, targetToMove.transform.rotation.z, targetToMove.transform.rotation.w);
            yield return null;
            currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / (animationDuration)) : (1.0f);
            print(currentTimePercentage);
        }
        mouseEnabled = true;
    }
    public IEnumerator SmoothPos2times(GameObject targetToMove, Vector3 a, Vector3 b, Vector3 c) //animation de 2 deplacements
    {
        mouseEnabled = false;
        float startTime = Time.realtimeSinceStartup;
        float currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / animationDuration) : (1.0f);
        while (currentTimePercentage <= 1.0f)
        {
            targetToMove.transform.position = Vector3.Lerp(a, b, animationCurve.Evaluate(currentTimePercentage));
            yield return null;
            currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / animationDuration) : (1.0f);
        }
        startTime = Time.realtimeSinceStartup;
        currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / animationDuration) : (1.0f);
        while (currentTimePercentage <= 1.0f)
        {
            targetToMove.transform.position = Vector3.Lerp(b, c, animationCurve.Evaluate(currentTimePercentage));
            yield return null;
            currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / animationDuration) : (1.0f);
        }
        mouseEnabled = true;
    }
}
