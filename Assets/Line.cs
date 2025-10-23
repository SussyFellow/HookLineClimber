using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    // Start is called before the first frame update
    LineRenderer lineRenderer;
    public GameObject startObject;
    public GameObject endObject;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // Set to draw a line between two points
    }

    void Update()
    {
        lineRenderer.SetPosition(0, startObject.transform.position);
        lineRenderer.SetPosition(1, new Vector3 (endObject.transform.position.x, endObject.transform.position.y - 0.05f, endObject.transform.position.z));
    }
}
