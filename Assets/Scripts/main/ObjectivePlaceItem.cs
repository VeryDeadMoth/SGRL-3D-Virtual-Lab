using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivePlaceItem : Objective
{
    public string container;
    public Dictionary<string, float> content;
    public bool melange;
    public string zone;
    public int place;
    public int numero;

    public ObjectivePlaceItem(string name, Dictionary<string, float> dictionary, bool mix,string zone, int place, int nb)
    {
        this.content = dictionary;
        this.container = name;
        this.melange = mix;
        this.zone = zone;
        this.place = place;
        this.numero = nb;
    }

    public override bool Evaluate(Objective obj)
    {
        Debug.Log(obj.GetType());
        if (obj.GetType() == typeof(ObjectivePlaceItem))
        {
            Debug.Log("1");
            ObjectivePlaceItem temp = (ObjectivePlaceItem)obj;

            bool flag = temp.content.Count == this.content.Count;
            
            if (flag)
            {
                foreach (KeyValuePair<string, float> pair in this.content)
                {

                    if (!(temp.content.ContainsKey(pair.Key)) || (pair.Value != -1 && !(temp.content[pair.Key] == pair.Value)))
                    {
                        Debug.Log(pair.Key);
                        Debug.Log(pair.Value);
                        Debug.Log(temp.content[pair.Key] );

                        flag = false;
                        break;
                    }
                }
            }

            //verification du reste
            return flag && this.container.Equals(temp.container) && this.melange == temp.melange && this.zone.Equals(temp.zone) && this.place == temp.place;
        }
        else
        {
            return false;
        }
        
    }

}
