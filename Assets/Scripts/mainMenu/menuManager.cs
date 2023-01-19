using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuManager : MonoBehaviour
{
    public GameObject LevelPanel;
    public GameObject creditPanel;
    public GameObject ActivPanel;

    public void b_credit()
    {
        if (ActivPanel != null)
        {
            ActivPanel.SetActive(false);
        }
        creditPanel.SetActive(true);
        ActivPanel = creditPanel;
    }
    public void b_level()
    {
        if (ActivPanel != null)
        {
            ActivPanel.SetActive(false);
        }
        LevelPanel.SetActive(true);
        ActivPanel = LevelPanel;
    }
    public void b_Close()
    {
        ActivPanel.SetActive(false);
    }

    public void b_quit()
    {
        Application.Quit();
    }

    public void b_LoadLevel(int i)
    {
        SceneManager.LoadScene(i);
    }
}
