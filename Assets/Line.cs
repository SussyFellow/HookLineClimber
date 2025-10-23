using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{

    LineRenderer lineRenderer;
    public GameObject startObject;
    public GameObject endObject;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        lineRenderer.SetPosition(0, startObject.transform.position); 
        lineRenderer.SetPosition(1, new Vector3 (endObject.transform.position.x, endObject.transform.position.y - 0.05f, endObject.transform.position.z));
        //draws the line slightly below the center of the hook so it lines up with the part of the sprite that has a little knot on it
    }
}
