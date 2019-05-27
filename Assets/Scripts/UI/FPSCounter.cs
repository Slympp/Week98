using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour {
    
    public TMP_Text Counter;

    private int    frameCount;
    private float  elapsedTime;
    private double frameRate;
    
    void Update() {
        frameCount++;
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 0.5f)
        {
            Counter.text = System.Math.Round(frameCount / elapsedTime, 1, System.MidpointRounding.AwayFromZero).ToString();
            frameCount = 0;
            elapsedTime = 0;
        }
    }
}
