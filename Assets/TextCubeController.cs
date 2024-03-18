using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TextCubeController : MonoBehaviour
{
    public GameObject textPlane;
    public int slideCountPerHalf = 20;
    public float spacing = 0.5f;
    public int middle = 0;

    List<GameObject> textPlanes = new List<GameObject>();
    List<float> textPlaneNominalYs = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        // Middle
        textPlanes.Add(Instantiate(textPlane, new Vector3(0, middle, 0), textPlane.transform.rotation));
        textPlaneNominalYs.Add(middle);

        // Top half
        for (var i = 1; i < slideCountPerHalf+1; i++)
        {
            float yPos = middle + i*spacing;
            textPlanes.Add(Instantiate(textPlane, new Vector3(0, yPos, 0), textPlane.transform.rotation));
            textPlaneNominalYs.Add(yPos);
        }

        // Bottom half
         for (var i = 1; i < slideCountPerHalf+1; i++)
        {
            float yPos = middle - i*spacing;
            textPlanes.Add(Instantiate(textPlane, new Vector3(0, yPos, 0), textPlane.transform.rotation));
            textPlaneNominalYs.Add(yPos);
        }
        
        textPlane.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time;
        for (var i = 0; i < textPlanes.Count; i++)
        {
            // textPlanes[i].transform.position = new Vector3(0, textPlaneNominalYs[i] + (float) (0.25*Math.Sin(t/5000.0)), 0);
            textPlanes[i].transform.localScale = new Vector3(1 + (float)(0.02*Math.Sin(t + textPlaneNominalYs[i])), 1, 1);
        }
        
    }
}
