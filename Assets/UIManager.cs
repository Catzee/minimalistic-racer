using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    //references
    public static UIManager instance;
    public TextMeshProUGUI speedometer;

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
        instance.speedometer.text = Mathf.RoundToInt(speed * 360000f / 1000f).ToString();
    }

}
