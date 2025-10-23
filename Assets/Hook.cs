using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public GameObject climber;
    public Rigidbody2D body;
    public AudioClip hookSound;
    AudioSource noise;

    void Awake()
    {
        noise = GetComponent<AudioSource>();
        body = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        noise.PlayOneShot(hookSound);
        climber.GetComponent<ProtagMovement>().isHooked = true;
        body.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
