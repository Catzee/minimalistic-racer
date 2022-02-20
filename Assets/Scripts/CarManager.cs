using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public float[] carSpeedStats;
    public float[] carTurnrateStats;

    public static CarManager instance;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

}
