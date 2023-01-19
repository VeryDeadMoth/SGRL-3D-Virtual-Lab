using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class CleaningScript : MonoBehaviour
{

    public Texture2D dirtMaskBase;
    public Texture2D brush;

    public Material material;

    Texture2D templateDirtMask;


    private void Start()
    {
        CreateTexture();
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

                for(int x=0; x < brush.width; x++)
                {
                    for (int y = 0; y < brush.height; y++)
                    {
                        Color pixelDirt = brush.GetPixel(x, y);
                        Color pixelDirtMask = templateDirtMask.GetPixel(pixelX + x, pixelY + y);


                        templateDirtMask.SetPixel(pixelX + x, pixelY + y, new Color(0, pixelDirtMask.g * pixelDirt.g, 0));
                    }
                }

                templateDirtMask.Apply();
            }
        }
    }

    void CreateTexture()
    {
        templateDirtMask = new Texture2D(dirtMaskBase.width, dirtMaskBase.height);
        templateDirtMask.SetPixels(dirtMaskBase.GetPixels());
        templateDirtMask.Apply();

        material.SetTexture("DirtTexture", templateDirtMask);
    }
}
*/