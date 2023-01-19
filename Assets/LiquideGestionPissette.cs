using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquideGestionPissette : MonoBehaviour
{
    public Transform pissette;
    public Material mat;
    public float animationDuration;
    public AnimationCurve animationCurve;
    bool mouseEnabled = true;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && mouseEnabled)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.transform == pissette)
                {
                    if (mat.GetFloat("_fill") < 0.1f)
                    {
                        StartCoroutine(setLiquid(mat.GetFloat("_fill"), 0.11f));
                    }
                    else
                    {
                        mat.SetFloat("_fill", (mat.GetFloat("_fill")+(0.10f*Time.deltaTime)));
                    }
                }
                
                
            }
        }
    }
    public IEnumerator setLiquid(float baseLiquid, float endLiquid)
    {
        mouseEnabled = false;
        float startTime = Time.realtimeSinceStartup;
        float currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / animationDuration) : (1.0f); // vide le liquide

        while (currentTimePercentage <= 1.0f)
        {
            mat.SetFloat("_fill", Mathf.Lerp(baseLiquid, endLiquid, animationCurve.Evaluate(currentTimePercentage)));
            yield return null;
            currentTimePercentage = (animationDuration > 0.0f) ? ((Time.realtimeSinceStartup - startTime) / animationDuration) : (1.0f);
        }
        mouseEnabled = true;
    }
}
