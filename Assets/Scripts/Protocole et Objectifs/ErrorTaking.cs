using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//***************************************************** PRELEVER 

[Serializable]

public class ErrorTaking : Error
{
    //not used yet
    public string message;

    public string container;
    public string tools;
    public string content;
    public int maxFill;
    public int place;

    public ErrorTaking(string m, string c, string t, string ct, int mx, int p)
    {
        this.message = m;
        this.container = c;
        this.tools = t;
        this.content = ct;
        this.maxFill = mx;
        this.place = p;
    }

    public override string ErrorMessage()
    {
        return this.message;
    }

    public override bool EvaluateError(Error error)
    {
        if (error.GetType() == typeof(ErrorTaking))
        {
            ErrorTaking temp = (ErrorTaking)error;
            bool flag = true;

            if (!this.container.Equals("$"))
            {
                flag = flag && this.container.Equals(temp.container);
            }
            if (!this.tools.Equals("$"))
            {
                flag = flag && this.tools.Equals(temp.tools);
            }
            if (!this.content.Equals("$"))
            {
                flag = flag && this.content.Equals(temp.content);
            }
            if (this.place != -1)
            {
                if (this.place == 1)
                {
                    flag = flag && this.place == temp.place;
                }
                else
                {
                    flag = flag && temp.place >= this.place;
                }

            }
            if (this.maxFill != -1)
            {
                flag = flag && this.maxFill > temp.maxFill;
            }

            return flag;
        }
        else
        {
            return false;
        }
    }
}
