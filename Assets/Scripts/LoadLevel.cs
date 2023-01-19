
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{

    public Scene level;
    public string nameLevel;

    public void LoadThisLevel(){
        SceneManager.LoadScene(nameLevel);
        
    }

}
