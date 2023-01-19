using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningScript : MonoBehaviour
{

    public Texture2D dirtMaskBase;
    public Texture2D brush;

    public Material material;

    Texture2D templateDirtMask;

    //dirt amount
    private float dirtAmountTotal;
    private float dirtAmount;

    //event
    public delegate void VoidEvents();
    public static event VoidEvents OnIsDoneCleaningEvent;


    private void OnEnable()
    {
        CreateTexture();
        gameObject.GetComponent<MeshCollider>().enabled = true;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector2 textureCoord = hit.textureCoord;

                int pixelX = (int)(textureCoord.x * templateDirtMask.width);
                int pixelY = (int)(textureCoord.y * templateDirtMask.height);

                //brush offset
                int pixelXOffset = pixelX - (brush.width / 2);
                int pixelYOffset = pixelY - (brush.height / 2);

                for (int x=0; x < brush.width; x++)
                {
                    for (int y = 0; y < brush.height; y++)
                    {
                        Color pixelDirt = brush.GetPixel(x, y);
                        Color pixelDirtMask = templateDirtMask.GetPixel(pixelXOffset + x, pixelYOffset + y);

                        //dirt amount
                        float removedAmount = pixelDirtMask.g - (pixelDirtMask.g * pixelDirt.g);
                        dirtAmount -= removedAmount;

                        templateDirtMask.SetPixel(pixelXOffset + x, pixelYOffset + y, new Color(0, pixelDirtMask.g * pixelDirt.g, 0));
                    }
                }

                templateDirtMask.Apply();
                //print(GetDirtAmount());

                if (IsClean()) //si l'objet est nettoyé, envoyer un message pour passer à l'objet suivant
                {
                    //send message that it's done
                    if (OnIsDoneCleaningEvent != null)
                    {
                        OnIsDoneCleaningEvent();
                        print("is done cleaning");
                        gameObject.GetComponent<MeshCollider>().enabled = false; // empeche collision plus tard
                    }
                    
                }
            }
        }
    }

    void CreateTexture()
    {
        templateDirtMask = new Texture2D(dirtMaskBase.width, dirtMaskBase.height);
        templateDirtMask.SetPixels(dirtMaskBase.GetPixels());
        templateDirtMask.Apply();

        material.SetTexture("_GreenMask", templateDirtMask);

        //dirt amount
        dirtAmountTotal = 0f;
        for (int x = 0; x < dirtMaskBase.width; x++)
        {
            for (int y = 0; y < dirtMaskBase.height; y++)
            {
                dirtAmountTotal += dirtMaskBase.GetPixel(x, y).g;
            }
        }
        dirtAmount = dirtAmountTotal;
    }

    private float GetDirtAmount()
    {
        return this.dirtAmount / dirtAmountTotal;
    }

    bool IsClean()
    {
        //***************** ATTENTION A BIEN CHANGER LE NOM DE L'OBJET QUI TIENT LE SCRIPT
        //print(gameObject.name);
        return (gameObject.name.Equals("Fiole") && GetDirtAmount() < 0.8) || (gameObject.name.Equals("Erlenmeyer") && GetDirtAmount() < 0.06) || (gameObject.name.Equals("Becher") && GetDirtAmount() < 0.06);

    }


}
