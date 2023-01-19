using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]

public class ErrorHolding : Error
{
    //not used yet
    public string message;

    //danger
    public int danger;
    //gant
    public bool? gloves;
    //bouchon 
    public bool? toxGlove;

    public bool? cap;

    //bool? est un nullable bool

    public ErrorHolding(string m, int d, bool g, bool t, bool l)
    {
        this.message = m;
        this.danger = d;
        this.gloves = g;
        this.toxGlove = t;
        this.cap = l;
    }

    //message pour popup
    public override string ErrorMessage()
    {
        return this.message;
    }


    public override bool EvaluateError(Error error)
    {
        if (error.GetType() == typeof(ErrorHolding))
        {
            ErrorHolding temp = (ErrorHolding)error;
            bool flag = true;

            if (this.danger != -1)
            {
                flag = flag && temp.danger == this.danger;
            }
            if(this.gloves != null)
            {
                flag = flag && this.gloves == temp.gloves;
            }
            if(this.cap != null)
            {
                flag = flag && this.cap == temp.cap;
            }
            if(this.toxGlove != null)
            {
                flag = flag && this.toxGlove == temp.toxGlove;
            }

            return flag;
        }
        else
        {
            return false;
        }
            
    }
}
