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
    void Update()   //it just moves in sync with the camera. 
                    //i realized i could reuse this script for the background when i was 
                    //implementing that so the background has the kilwall script.
                    //killwalls are just offscreen invisible collider boxes that prevent you from falling out of the stage to the left or right
    {
        transform.position = new Vector3(transform.position.x + mainScript.cameraSpeed * Time.deltaTime, transform.position.y, transform.position.z);
    }
}
