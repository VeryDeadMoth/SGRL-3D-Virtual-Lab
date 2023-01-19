using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorWaterInAcid : Error
{
    Dictionary<string, int> elementsPouredIn;
    Dictionary<string, int> elementsAlreadyPresent;
    public ErrorWaterInAcid(Dictionary<string,int> pouredIn, Dictionary<string,int> alreadyIn)
    {
        this.elementsAlreadyPresent = alreadyIn;
        this.elementsPouredIn = pouredIn;
    }

    //Message pour le popup
    public override string ErrorMessage()
    {
        return "Attention ! Il ne faut pas mettre l'eau dans l'acide.";
    }


    //NE PREND EN COMPTE QUE SI ON VERSE SEULEMENT DE L'EAU DANS L'ACIDE (sans eau avec)
    //(eau avec ou sans autres éléments ?)
    //acide ici changé par poudre pour test
    public override bool EvaluateError(Error error)
    {
        
        if (error.GetType() == typeof(ErrorWaterInAcid))
        {
            
            ErrorWaterInAcid temp = (ErrorWaterInAcid)error;
            if (temp.elementsPouredIn.ContainsKey("eau distillée") && temp.elementsAlreadyPresent.ContainsKey("poudre") && !temp.elementsAlreadyPresent.ContainsKey("eau distillée"))
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


}
