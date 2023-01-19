using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]

public class ErrorLid : Error
{
    //not used yet
    public string message;

    public int danger;
    public int place;

    public ErrorLid(string m, int d, int p)
    {
        this.message = m;
        this.danger = d;
        this.place = p;
    }

    public override string ErrorMessage()
    {
        return this.message;
    }

    public override bool EvaluateError(Error error)
    {
        if (error.GetType() == typeof(ErrorLid))
        {
            ErrorLid temp = (ErrorLid)error;
            bool flag = true;
            if (this.danger != -1)
            {
                flag = flag && temp.danger == this.danger;
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

            return flag;
        }
        else
        {
            return false;
        }
    }

}
