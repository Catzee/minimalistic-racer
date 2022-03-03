using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    //references
    public static UIManager instance;
    public TextMeshProUGUI speedometer;
    public TextMeshProUGUI laptimeValue;
    public TextMeshProUGUI highscoreValue;

    //internal
    private static int speedCached = -1;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetSpeedometer(float speed)
    {
        int roundedSpeed = Mathf.RoundToInt(speed * 360000f / 1000f);
        if(roundedSpeed != speedCached) //minimize string allocations
        {
            instance.speedometer.text = roundedSpeed.ToString();
        }
    }

    public static void SetLaptimeValue(float time)
    {
        instance.laptimeValue.text = time.ToString("F1");
    }

    public static void SetHighscoreValue(float time)
    {
        string highscoreText = time.ToString("F1");
        if(time <= 0f)
        {
            highscoreText = "None";
        }
        instance.highscoreValue.text = highscoreText;
    }

}
