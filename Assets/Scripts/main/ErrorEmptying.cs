using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]

public class ErrorEmptying : Error
{
    //not used yet
    public string message;

    public int danger;
    public string trashCan;

    public ErrorEmptying(string m, int d, string t)
    {
        this.message = m;
        this.danger = d;
        this.trashCan = t;
    }

    public override string ErrorMessage()
    {
        return this.message;
    }

    public override bool EvaluateError(Error error)
    {
        if (error.GetType() == typeof(ErrorEmptying))
        {
            ErrorEmptying temp = (ErrorEmptying)error;
            bool flag = true;

            if (!this.trashCan.Equals("$"))
            {
                flag = flag && this.trashCan.Equals(temp.trashCan);
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
