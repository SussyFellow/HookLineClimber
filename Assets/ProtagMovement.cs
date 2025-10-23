using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProtagMovement : MonoBehaviour
{
    [Header("Variables")]
    public float speed; //strictly speaking this variable controls acceleration
    public Vector2 boxSize; //for setting up the groundedness detection
    public float castDistance; //also for groundedness detection
    public float pullPower; //how hard does your grapple hook pull you
    public float throwPower; //how hard do you throw your hook
    public float speedCap; //just caps like, normal controlled left-right movement. the hook can get you way faster no problem

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

    void normalMove() //you can move left or right by pressing A or D respectively.
    {
        if (Input.GetKey(KeyCode.A) && body.velocity.x > -speedCap) //lets you build momentum with normal movement up to a cap, if you wanna go faster you'll need to use the hook
        {
            body.AddForce(new Vector2(-speed * Time.deltaTime, 0));
        }
        else if (Input.GetKey(KeyCode.D) && body.velocity.x < speedCap)
        {
            body.AddForce(new Vector2(speed * Time.deltaTime, 0));
        }
    }

    public bool isGrounded() //checks if you're on the ground. simple enough.
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

    private void OnDrawGizmos() //this visualizes the groundedness detection box in the editor.
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject otherThing = collision.gameObject;
        noise.PlayOneShot(contactSound); //every time you touch a platform it makes a noise. because youre hitting a stone platform.
        if (otherThing.CompareTag("Deadly")) //touch spikes (or offscreen deathwalls) and die.
        {
            mainScript.GameLoss();
        }
        if (isGrounded())   //changing your material to be frictionless unless youre on the ground
                            //feels like a hack but it works and doesn't seem to cause issues so i don't care
                            //prevents sticking to sides of platforms
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
        if (isHooked) //if your hook is successfully grapped onto something, you can gain acceleration from it.
                    //otherwise youre just pulling it in for another throw
        {
            isHooked = false; //not hooked anymore
            Vector2 direction = new Vector2(currentHook.transform.position.x - transform.position.x, 
                                            currentHook.transform.position.y - transform.position.y);
            body.AddForce(direction.normalized * pullPower);
        }
        Destroy(currentHook);
        Destroy(currentLine);
    }

    void updateSprite()
    {
        if (isGrounded()) //for grounded sprites
        {
            if (Math.Abs(body.velocity.x) < 0.1)//if youre either still or extremely slow, you are standing
            {
                spriteRender.sprite = standSprite;
            }
            else if (stepCounting > 0.1) //if its been 0.1 seconds since your last step, take another one
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
            else //otherwise, count how long since your last step
            {
                spriteRender.sprite = currentWalk;
                stepCounting += Time.deltaTime;
            }

        }
        else //if not grounded, youre in the air. use the sprite for when youre in the air
        {
            spriteRender.sprite = airSprite;
        }
    }

}
