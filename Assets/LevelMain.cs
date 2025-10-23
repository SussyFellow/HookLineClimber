using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMain : MonoBehaviour
{
    public string lostScene;
    public List<List<GameObject>> slices;
    public GameObject platform;
    public GameObject spike;
    public float cameraStartSpeed;
    public float cameraSpeed;
    float lastSliceX;
    float threshold;

    void Awake()
    {
        cameraSpeed = cameraStartSpeed;
        lastSliceX = 2;
        slices = new List<List<GameObject>>();
        threshold = 20;
        for (int i = -12; i < 3; i++) //builds out the level behind and below your starting position
        {
            BuildSlice(i, "");
        }
    }

    void Update()
    {  
        //moves the camera
        transform.position = new Vector3(transform.position.x + cameraSpeed * Time.deltaTime, transform.position.y, transform.position.z);

        sliceHandler(); //regularly builds the rest of the level ahead of you

        if (transform.position.x > threshold) //speeds up the camera over time. eventually the game gets impossible, in theory,
                                            //but it should take a good long while until it gets there at this rate.
        {
            threshold = threshold * 1.5f;
            cameraSpeed = cameraSpeed * 1.1f;
        }
    }

    void sliceHandler() //builds out the level in vertical slices
    {
        if (transform.position.x + 12 > lastSliceX) //+12 is just like, a decent ways offscreen right
        {
            string sliceType;
            int opt = Random.Range(1, 5);
            if (lastSliceX % 5 == 0 || opt == 4) //guarantees you a normal platform once every five slices, 1/5 chance otherwise
            {
                sliceType = "platform";
            }
            else if (opt < 3) //2 outta 5 chance for a spike platform
            {
                sliceType = "spike";
            }
            else //remaining 2/5 chance is for no platform in a given slice. theorietical ideal run would always hit this
            {
                sliceType = "";
            }

            lastSliceX++;

            BuildSlice(lastSliceX, sliceType); //builds out next slice
        }

        if (slices.Count > 25) //checks for if theres enough slices to fill out the screen and then some
        {
            while (slices[0].Count > 0) //removes slices offscreen to the left
            {
                GameObject tmp = slices[0][0];
                slices[0].RemoveAt(0);
                Destroy(tmp);
            }
            slices.RemoveAt(0);
        }
    }

    public void BuildSlice(float x, string sliceType) //builds slices
    {
        float platY = Random.Range(-4, 5) - 0.5f;
        List<GameObject> newSlice = new List<GameObject>();
        slices.Add(newSlice);
        BuildTopBottom(x, newSlice); //delegated to helper method
        switch (sliceType)
        {
            case "spike": //platform with three spikes each on the bottom and top
                newSlice.Add(Instantiate(platform, new Vector3(x, platY, 0f), Quaternion.identity));
                newSlice.Add(Instantiate(spike, new Vector3(x + 0.33f, platY + 0.25f, 0f), Quaternion.identity));
                newSlice.Add(Instantiate(spike, new Vector3(x, platY + 0.25f, 0f), Quaternion.identity));
                newSlice.Add(Instantiate(spike, new Vector3(x - 0.33f, platY + 0.25f, 0f), Quaternion.identity));
                newSlice.Add(Instantiate(spike, new Vector3(x - 0.33f, platY - 0.25f, 0f), new Quaternion(0f, 0f, 180f, 0f)));
                newSlice.Add(Instantiate(spike, new Vector3(x, platY - 0.25f, 0f), new Quaternion(0f, 0f, 180f, 0f)));
                newSlice.Add(Instantiate(spike, new Vector3(x + 0.33f, platY - 0.25f, 0f), new Quaternion(0f, 0f, 180f, 0f)));
                break;
            case "platform": //normal platform
                newSlice.Add(Instantiate(platform, new Vector3(x, platY, 0f), Quaternion.identity));
                break;
            default: //no platform
                break;
        }
    }

    void BuildTopBottom(float x, List<GameObject> newSlice) //spikey roof and floor for danger
    {
        newSlice.Add(Instantiate(platform, new Vector3(x, -5f, 0f), Quaternion.identity));
        newSlice.Add(Instantiate(platform, new Vector3(x, 5f, 0f), Quaternion.identity));

        newSlice.Add(Instantiate(spike, new Vector3(x + 0.33f, 4.75f, 0f), new Quaternion(0f, 0f, 180f, 0f)));
        newSlice.Add(Instantiate(spike, new Vector3(x, 4.75f, 0f), new Quaternion(0f, 0f, 180f, 0f)));
        newSlice.Add(Instantiate(spike, new Vector3(x - 0.33f, 4.75f, 0f), new Quaternion(0f, 0f, 180f, 0f)));

        newSlice.Add(Instantiate(spike, new Vector3(x + 0.33f, -4.75f, 0f), Quaternion.identity));
        newSlice.Add(Instantiate(spike, new Vector3(x, -4.75f, 0f), Quaternion.identity));
        newSlice.Add(Instantiate(spike, new Vector3(x - 0.33f, -4.75f, 0f), Quaternion.identity));
    }
    
    public void GameLoss() //takes you to the gameloss screen.
        {
            SceneManager.LoadScene(lostScene);
        }

}
