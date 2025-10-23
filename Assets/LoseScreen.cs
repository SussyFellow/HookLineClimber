using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen : MonoBehaviour
{

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
