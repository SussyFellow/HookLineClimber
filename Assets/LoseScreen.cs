using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen : MonoBehaviour
{
    //the lose screen just shows you the score you got, your high score, and a button to return to the title screen

    public string titleScreen;
    public AudioClip loseSound;
    AudioSource noise;

    void Awake() 
    {
        noise = GetComponent<AudioSource>();
        noise.PlayOneShot(loseSound);
    }

    public void LoadTitle() 
    {
        SceneManager.LoadScene(titleScreen);
    }
}
