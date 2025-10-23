using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillWall : MonoBehaviour
{
    LevelMain mainScript;
    
    // Start is called before the first frame update
    void Start()
    {
        mainScript = Camera.main.GetComponent<LevelMain>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + mainScript.cameraSpeed * Time.deltaTime, transform.position.y, transform.position.z);
    }
}
