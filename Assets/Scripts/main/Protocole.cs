using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Protocole
{
    //pour la fin du jeu 
    public Scene level;
    public string nameLevel = "fin";
    //Events -> notification pour les toggles 
    public delegate void ObjectiveSuccessfullyCompleted();
    public static event ObjectiveSuccessfullyCompleted OnObjectiveSuccessfullyCompletedEvent;
    //On peut deplacer la gestion des toggles ici si on a le meme nombre de toggles et d'objectifs (toggleList, voir Test_Level_Manager) (?)

    //Dictionnaire des objectifs à faire+ un bool associé à chaque objectif pour savoir si il a été accompli 
    public Dictionary<Objective,bool> dictionaryOfObjectives = new Dictionary<Objective, bool>();
    //remplacer par liste ?
    //Liste des objectifs dans l'ordre
    public List<Objective> listOfObjectives = new List<Objective>();

    //Nb d'objectifs effectués (<= nb objectifs dans la liste)
    public int objectivesCounter =0;

    //************************************************************
    //variables pour detection des erreurs

    //events -> pour activation popup
    public delegate void ErrorDetected(string message);
    public static event ErrorDetected OnErrorDetectedEvent;

    //liste des erreurs + le nombre de fois qu'elles ont été faites
    public Dictionary<Error, int> dictionaryOfErrors = new Dictionary<Error, int>();


    //Regarde si l'action effectuée est l'objectif à faire
    //Si oui, update du dictionnaire + incrementation compteur
    //Seulement pour objectifs ordonnés

    //pour le build
    private string typeobj;
    private ObjectivePlaceItem objPrime;
    private string nameContainer; 

    public void checkIfOrderedObjectiveIsValidated(Objective obj)
    {
        //Debug.Log(objectivesCounter);


        /*typeobj = obj.GetType().ToString();

        if (typeobj == "ObjectivePlaceItem")
        {
            objPrime = (ObjectivePlaceItem)obj;
            Debug.Log(objPrime.content);
            nameContainer = objPrime.container.ToString();
            if ( nameContainer == "fiole100" /*&& objPrime.content == [])
            {

            }
            
        }*/

        //Debug.Log(listOfObjectives[objectivesCounter].Evaluate(obj));
        //Enlever premiere condition dans premier if si terminer le protocole empeche les interactions plus tard qui causent des index out of range

        Debug.Log(listOfObjectives[objectivesCounter].GetType());
        if (objectivesCounter < listOfObjectives.Count && listOfObjectives[objectivesCounter].Evaluate(obj)) //check si objectif ok
        {

            //Do something - Notifier les toggles
            Debug.Log("Notify toggles");
            if (OnObjectiveSuccessfullyCompletedEvent != null)
            {
                OnObjectiveSuccessfullyCompletedEvent();
            }
            
            dictionaryOfObjectives[obj] = true;
            objectivesCounter++;

            Debug.Log(listOfObjectives.Count);
            if (objectivesCounter == 7)
            {
                LoadThis();   
            }
        }
    }

    //******************************************************************************
    //Fonction pour detection des erreurs 

   public bool checkIfErrorWasDoneFill(Error error, List<ErrorFilling> listOfErrors,int nbOfElements) // MAY BE REMOVED
    {
        bool flag = false;
        foreach(ErrorFilling errorFilling in listOfErrors)
        {
            if((!errorFilling.pouredIn.Equals("acide"))||(errorFilling.pouredIn.Equals("acide") && nbOfElements == 1))
            {
                if (errorFilling.EvaluateError(error))
                {
                    if (dictionaryOfErrors.ContainsKey(errorFilling))
                    {
                        dictionaryOfErrors[errorFilling] += 1;
                    }
                    else
                    {
                        dictionaryOfErrors.Add(errorFilling, 1);
                    }

                    if (OnErrorDetectedEvent != null)
                    {
                        OnErrorDetectedEvent(errorFilling.ErrorMessage());
                    }
                    flag = true;
                }
            }
            
        }
        return flag;
    }
    //pour chnager de scene 
    public void LoadThis()
    {
        SceneManager.LoadScene(nameLevel);

    }
    public bool CheckFillErrors(Error error, List<ErrorFilling> listOfErrors) //if true -> il y a eu une erreur
    {
        bool flag = false;
        foreach (ErrorFilling errorFilling in listOfErrors)
        {
            if (errorFilling.EvaluateError(error))
            {
                flag = true;
            }
        }

        return flag;
    }

    public bool CheckLidErrors(Error error, List<ErrorLid> listOfErrors)
    {
        bool flag = false;
        foreach (ErrorLid errorLid in listOfErrors)
        {
            if (errorLid.EvaluateError(error))
            {
                flag = true;
            }
        }

        return flag;
    }


    //****************************************************************************

    //-> deserialisation du fichier json contenant les etapes du protocole

    public void DeserializeJSONProtocole(TextAsset json)
    {
        /*string str = "{\"dictionaryOfElementsAndQuantityRequired\":{\"poudre\" : 20 }}"; /*"{\"tagOrNameOfObject\" : \"Fiole\"}";
        //Debug.Log(str);
        return Newtonsoft.Json.JsonConvert.DeserializeObject<ObjectiveContainsDictionary>(str);*/

        ObjectiveManager objectiveManager = Newtonsoft.Json.JsonConvert.DeserializeObject<ObjectiveManager>(json.text);
        int counterMax = objectiveManager.Contient.Count + objectiveManager.Put.Count + objectiveManager.take.Count;
        int counter = 1;
        while (counter < counterMax+1)
        {
            foreach(ObjectiveContains obj in objectiveManager.Contient)
            {
                if (obj.numero == counter)
                {
                    listOfObjectives.Add(obj);
                    dictionaryOfObjectives.Add(obj, false);
                    counter++;
                }
            }
            foreach (ObjectivePlaceItem obj in objectiveManager.Put)
            {
                if (obj.numero == counter)
                {
                    listOfObjectives.Add(obj);
                    dictionaryOfObjectives.Add(obj, false);
                    counter++;
                }
            }
            foreach (ObjectiveGrabItem obj in objectiveManager.take)
            {
                if (obj.numero == counter)
                {
                    listOfObjectives.Add(obj);
                    dictionaryOfObjectives.Add(obj, false);
                    counter++;
                }
            }
        }

    }

    //-> deserialisation du fichier json contenant les erreurs possibles
    public ErrorManager DeserializeJSONErrors(TextAsset json)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorManager>(json.text);
    }
}
