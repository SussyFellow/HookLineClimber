using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProtagMovement : MonoBehaviour
{
    [Header("Variables")]
    public float speed;
    public Vector2 boxSize;
    public float castDistance;
    public float pullPower;
    public float throwPower;
    public float speedCap;

    [Header("Sounds")]
    public AudioClip throwSound;
    public AudioClip retrieveSound;
    public AudioClip contactSound;

    [Header("Sprites")]
    public Sprite standSprite;
    public Sprite walkLeftSprite;
    public Sprite walkRightSprite;
    public Sprite airSprite;

    [Header("Do not change")]
    public PhysicsMaterial2D grippy;
    public PhysicsMaterial2D slippy;
    public GameObject hookPrefab;
    GameObject currentHook;
    public bool isHooked;
    public GameObject linePrefab;
    public LayerMask groundLayer;

    private Rigidbody2D body;
    private float stepCounting;
    private Sprite currentWalk;
    GameObject currentLine;
    LevelMain mainScript;
    AudioSource noise;
    SpriteRenderer spriteRender;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        noise = GetComponent<AudioSource>();
        spriteRender = GetComponent<SpriteRenderer>();
        currentWalk = walkLeftSprite;
        isHooked = false;
        mainScript = Camera.main.GetComponent<LevelMain>();
        stepCounting = 0.0f;
    }

    void Update()
    {
        normalMove(); //normal movement stuff

        if (Input.GetMouseButtonDown(0)) //when lmb clicked
        {
            if (currentHook)
            {
                noise.PlayOneShot(retrieveSound);
                removeHook();
            }
            else
            {
                noise.PlayOneShot(throwSound);
                hookThrow();
            }
        }

        if (body.position.x > ScoreCounter.score + 1)
        {
            ScoreCounter.score = (int)body.position.x;
            HighScore.TRY_SET_HIGH_SCORE(ScoreCounter.score);
        }

        updateSprite();
    }

    void normalMove()
    {
        if (Input.GetKey(KeyCode.A) && body.velocity.x > -speedCap)
        {
            body.AddForce(new Vector2(-speed * Time.deltaTime, 0));
        }
        else if (Input.GetKey(KeyCode.D) && body.velocity.x < speedCap)
        {
            body.AddForce(new Vector2(speed * Time.deltaTime, 0));
        }
    }

    public bool isGrounded()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject otherThing = collision.gameObject;
        noise.PlayOneShot(contactSound);
        if (otherThing.CompareTag("Deadly"))
        {
            mainScript.GameLoss();
        }
        if (isGrounded())
        {
            body.sharedMaterial = grippy;
        }
        else
        {
            body.sharedMaterial = slippy;
        }
    }

    void hookThrow()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //get mouse pos
        Vector2 direction = new Vector2(mouseWorldPos.x - transform.position.x, mouseWorldPos.y - transform.position.y); //get direction vector from protag to mouse
        currentHook = Instantiate(hookPrefab, transform.position, Quaternion.identity); //create hook
        currentHook.GetComponent<Hook>().climber = this.gameObject; //make sure hook knows who threw it
        Rigidbody2D hookBody = currentHook.GetComponent<Rigidbody2D>(); //get hook rigidbody
        hookBody.velocity = direction.normalized * throwPower; //set hook's velocity using direction vector

        currentLine = Instantiate(linePrefab, transform.position, Quaternion.identity); //make the line between you and the hook
        Line lineScript = currentLine.GetComponent<Line>();
        lineScript.startObject = this.gameObject; //set start and end points to you and the hook
        lineScript.endObject = currentHook;
    }

    void removeHook()
    {
        if (isHooked)
        {
            isHooked = false; //not hooked anymore
            Vector2 direction = new Vector2(currentHook.transform.position.x - transform.position.x, currentHook.transform.position.y - transform.position.y);
            body.AddForce(direction.normalized * pullPower);
        }
        Destroy(currentHook);
        Destroy(currentLine);
    }

    void updateSprite()
    {
        if (isGrounded())
        {
            if (Math.Abs(body.velocity.x) < 0.1)
            {
                spriteRender.sprite = standSprite;
            }
            else if (stepCounting > 0.1)
            {
                stepCounting = 0.0f;
                if (currentWalk == walkLeftSprite)
                {
                    currentWalk = walkRightSprite;
                }
                else
                {
                    currentWalk = walkLeftSprite;
                }
                spriteRender.sprite = currentWalk;
            }
            else
            {
                spriteRender.sprite = currentWalk;
                stepCounting += Time.deltaTime;
            }

        }
        else
        {
            spriteRender.sprite = airSprite;
        }
    }

}
