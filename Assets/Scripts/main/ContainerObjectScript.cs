using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerObjectScript : MonoBehaviour
{
    //************************************************************* VARIABLES

    //event -> pour les objectifs
    public delegate void ObjectHadSomethingHappen(Objective objective);
    public static event ObjectHadSomethingHappen ObjectHadSomethingHappenEvent;

    //nom (may be removed if directly name of game object)
    public string containerName;

    //bouchon
    public bool needsCap;
    public bool capIsOn = false;
    public GameObject cap;

    //danger
    public int danger;

    //melange
    public bool wasMixed = false;

    //fill (% ?) (taux de remplissage) -> float ?
    public int fill;
    public float shaderFill;
    public Material material;

    //contient
    public Dictionary<string, float> elementsContained;

    //pH (?)
    public string pH;

    //placeholder it occupies
    public GameObject hiddenPlaceholder;

    //poids en g
    public float weight;

    //weight goal -> -1 si doit etre ignoré
    public float weightGoal;



    //peut-on prelever ?
    //add bool ?
    //si a deja prelevé -> string du premier prelevement pour erreurs prelevement

    // stuff pour la version build 

    //bool pour savoir si on verse quelque chose 
    public bool didFill = false;
    public bool didTake = false;


    //************************************************************* FONCTIONS

    public void Start()
    {
        elementsContained = new Dictionary<string, float>();
        if (material != null)
        {
            material.SetFloat("_fill", shaderFill);
        }
    }

    public void CapInteraction()
    {
        if (needsCap)
        {
            capIsOn = !capIsOn;
            cap.SetActive(capIsOn);
        }
    }

    public void GrabObject()
    {
        ObjectiveGrabItem objgi = new ObjectiveGrabItem(containerName,-1);
        if (ObjectHadSomethingHappenEvent != null)
        {
            ObjectHadSomethingHappenEvent(objgi);
        }

    }

    public void DropObject(string zone,int place)
    {
        ObjectivePlaceItem objgi = new ObjectivePlaceItem(containerName,elementsContained,wasMixed,zone,place, -1);
        if (ObjectHadSomethingHappenEvent != null)
        {
            Debug.Log(objgi.container);
            ObjectHadSomethingHappenEvent(objgi);
        }
    }

    public void FillObject(string putInName,float putInQuant,float fillQuantity,bool asWeight) //remplir -> fill quantity pour shader seulement / asWeight -> mettre la quantité en temps que poids
    {
        
        if((needsCap && !capIsOn)||!needsCap)
        {
            
            didFill = true; // on dit que la fonction c'est enclenchée

            //poids
            if (weightGoal != -1)
            {
                if (weight >= weightGoal)
                {
                    weight += (weight / 2);
                }
                else //ok
                {
                    if (Mathf.Abs(weightGoal - weight) < 2)
                    {
                        weight = weightGoal;
                    }
                    else
                    {
                        float temp = weightGoal - weight;
                        weight += Random.Range((temp - temp * 1 / 5), (temp + temp * 1 / 5));
                    }

                }
            }

            //ajout element + quantité dans dico
            if (!this.elementsContained.ContainsKey(putInName))
            {
                if (asWeight)
                {
                    this.elementsContained.Add(putInName, weight);
                }
                else
                {
                    this.elementsContained.Add(putInName, putInQuant);
                }

            }
            else
            {
                if (asWeight)
                {
                    this.elementsContained[putInName] = weight;
                }
                else
                {
                    this.elementsContained[putInName] += putInQuant;
                }

            }

            //gestion shader
            if (material != null)
            {
                shaderFill += fillQuantity;
                LeanTween.value(gameObject, UpdateShaderFill, material.GetFloat("_fill"), shaderFill, 0.5f).setEaseOutCubic();

            }

            ObjectiveContains objcd = new ObjectiveContains(containerName, elementsContained, wasMixed, -1);
            if (ObjectHadSomethingHappenEvent != null)
            {
                ObjectHadSomethingHappenEvent(objcd);
            }
        }
        
    }

    public void TakeFromObject(float takeFromQuantity, float fillQuantity,bool asWeight)
    {
        if ((needsCap && !capIsOn) || !needsCap)
        {
            /*if (elementsContained.Count==1)
        {
            elementsContained[KEY] -= takeFromQuantity;
        }*/
            didTake = true;
            //gestion shader
            if (material != null)
            {
                shaderFill -= fillQuantity;
                if (shaderFill < 0)
                {
                    shaderFill = 0;
                    LeanTween.value(gameObject, UpdateShaderFill, material.GetFloat("_fill"), shaderFill, 0.5f).setEaseOutCubic();
                }
            }

            //poids
            if (weightGoal != -1)
            {
                if (weight <= weightGoal)
                {
                    weight -= (weight / 2);
                }
                else //ok
                {
                    if (Mathf.Abs(weightGoal - weight) < 2)
                    {
                        weight = weightGoal;
                    }
                    else
                    {
                        float temp = weightGoal - weight;
                        weight += Random.Range((temp - temp * 1 / 5), (temp + temp * 1 / 5));
                    }
                }

                if (weight < 0)
                {
                    weight = 0;
                }
            }

            if (asWeight)
            {
                string key ="";
                if (elementsContained.Count == 1)
                {
                    foreach(KeyValuePair<string, float> pair in elementsContained)
                    {
                        key = pair.Key;
                    }
                    elementsContained[key] = weight;
                }
            }

            ObjectiveContains objcd = new ObjectiveContains(containerName, elementsContained, wasMixed, -1);
            if (ObjectHadSomethingHappenEvent != null)
            {
                ObjectHadSomethingHappenEvent(objcd);
            }
        }
        
        
    }

    public void UpdateShaderFill(float newValue)
    {
        material.SetFloat("_fill", newValue);
    }

    public void EmptyObject()
    {
        if ((needsCap && !capIsOn) || !needsCap)
        {
            elementsContained.Clear();
            shaderFill = 0;
            fill = 0;
            danger = 0;
            pH = "neutre";
            weight = 0;

            if (material != null)
            {
                material.SetFloat("_fill", shaderFill);
            }
        }
        
    }

}
