using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]

public class ErrorFilling : Error
{
    //not used yet
    public string message;

    public string container;
    //what is already in
    public string pouredIn;
    //new added thing
    public string newElement;
    public int danger;
    public int place;
    public int maxFill;
    public bool? mix;

    public ErrorFilling(string m, string c, string pr, string n, int d, int p, int mx, bool mi)
    {
        this.message = m;
        this.container = c;
        this.pouredIn = pr;
        this.newElement = n;
        this.maxFill = mx;
        this.place = p;
        this.mix = mi;
        this.danger = d;
    }

    public override string ErrorMessage()
    {
        return this.message;
    }

    public override bool EvaluateError(Error error)
    {
        if (error.GetType() == typeof(ErrorFilling))
        {
            ErrorFilling temp = (ErrorFilling)error;
            bool flag = true;

            if (!this.container.Equals("$"))
            {
                flag = flag && this.container.Equals(temp.container);
            }
            if (!this.pouredIn.Equals("$"))
            {
                flag = flag && this.pouredIn.Equals(temp.pouredIn);
            }
            if (!this.newElement.Equals("$"))
            {
                flag = flag && this.newElement.Equals(temp.newElement);
            }
            if(this.mix != null)
            {
                flag = flag && this.mix == temp.mix;
            }
            if (this.place != -1)
            {
                if (this.place == 2)
                {
                    flag = flag && this.place == temp.place;
                }
                else
                {
                    flag = flag && temp.place <= this.place; 
                }
                
            }
            if (this.maxFill != -1)
            {
                flag = flag && this.maxFill > temp.maxFill;
            }
            if (this.danger != -1)
            {
                flag = flag && temp.danger == this.danger;
            }

            return flag;
        }
        else
        {
            return false;
        }

    }
}
