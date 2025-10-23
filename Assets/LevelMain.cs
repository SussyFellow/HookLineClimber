using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMain : MonoBehaviour
{
    public string lostScene;
    public bool lost;
    public List<List<GameObject>> slices;
    public GameObject platform;
    public GameObject spike;
    public float cameraStartSpeed;
    public float cameraSpeed;
    float lastSliceX;
    float threshold;

    void Awake()
    {
        lost = false;
        cameraSpeed = cameraStartSpeed;
        lastSliceX = 2;
        slices = new List<List<GameObject>>();
        threshold = 20;
        for (int i = -12; i < 3; i++)
        {
            BuildSlice(i, "");
        }
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x + cameraSpeed * Time.deltaTime, transform.position.y, transform.position.z);

        sliceHandler();

        if (transform.position.x > threshold)
        {
            threshold = threshold * 1.5f;
            cameraSpeed = cameraSpeed * 1.1f;
        }
    }

    void sliceHandler()
    {
        if (transform.position.x + 12 > lastSliceX)
        {
            string sliceType;
            int opt = Random.Range(1, 5);
            if (lastSliceX % 5 == 0 || opt == 4)
            {
                sliceType = "platform";
            }
            else if (opt < 3)
            {
                sliceType = "spike";
            }
            else
            {
                sliceType = "";
            }

            lastSliceX++;

            BuildSlice(lastSliceX, sliceType);
        }

        if (slices.Count > 25)
        {
            while (slices[0].Count > 0)
            {
                GameObject tmp = slices[0][0];
                slices[0].RemoveAt(0);
                Destroy(tmp);
            }
            slices.RemoveAt(0);
        }
    }

    public void BuildSlice(float x, string sliceType)
    {
        float platY = Random.Range(-4, 5) - 0.5f;
        List<GameObject> newSlice = new List<GameObject>();
        slices.Add(newSlice);
        BuildTopBottom(x, newSlice);
        switch (sliceType)
        {
            case "spike":
                newSlice.Add(Instantiate(platform, new Vector3(x, platY, 0f), Quaternion.identity));
                newSlice.Add(Instantiate(spike, new Vector3(x + 0.33f, platY + 0.25f, 0f), Quaternion.identity));
                newSlice.Add(Instantiate(spike, new Vector3(x, platY + 0.25f, 0f), Quaternion.identity));
                newSlice.Add(Instantiate(spike, new Vector3(x - 0.33f, platY + 0.25f, 0f), Quaternion.identity));
                newSlice.Add(Instantiate(spike, new Vector3(x - 0.33f, platY - 0.25f, 0f), new Quaternion(0f, 0f, 180f, 0f)));
                newSlice.Add(Instantiate(spike, new Vector3(x, platY - 0.25f, 0f), new Quaternion(0f, 0f, 180f, 0f)));
                newSlice.Add(Instantiate(spike, new Vector3(x + 0.33f, platY - 0.25f, 0f), new Quaternion(0f, 0f, 180f, 0f)));
                break;
            case "platform":
                newSlice.Add(Instantiate(platform, new Vector3(x, platY, 0f), Quaternion.identity));
                break;
            default:
                break;
        }
    }

    void BuildTopBottom(float x, List<GameObject> newSlice)
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
    
    public void GameLoss()
        {
            SceneManager.LoadScene(lostScene);
        }

}
