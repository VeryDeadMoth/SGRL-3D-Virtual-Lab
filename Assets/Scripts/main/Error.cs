using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Error
{
    public abstract bool EvaluateError(Error error);

    public abstract string ErrorMessage();
}
