using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public GameObject m_StartButton;
    public GameObject m_Screen;
    public GameObject m_minus;
    public GameObject m_plus;

    void Start()
    {
        m_StartButton.SetActive(true);
        m_Screen.SetActive(false);
        m_minus.SetActive(false);
        m_plus.SetActive(false);
    }
}
