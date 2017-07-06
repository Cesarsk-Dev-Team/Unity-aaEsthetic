using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    //static variables when you change the scene are not reset
    public static int PinCount = 0;

    public Text scoreText;

    void Start()
    {
        PinCount = 0;
        scoreText.text = "";
    }

    void Update()
    {
        if(PinCount != 0) scoreText.text = PinCount.ToString();
    }
}
