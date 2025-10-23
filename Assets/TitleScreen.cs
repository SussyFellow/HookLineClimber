using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    public string levelScene;

    public void PlayGame()
    {
        SceneManager.LoadScene(levelScene);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
