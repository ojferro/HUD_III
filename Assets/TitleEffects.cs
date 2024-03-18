using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TitleEffects : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshPro textmeshPro;
    public float nominalFontSize = 6.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textmeshPro.fontSize = nominalFontSize + (float)(0.5*Math.Sin(Time.time*0.1f));
    }
}
