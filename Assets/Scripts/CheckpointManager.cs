using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static Transform[] trackCheckpoints;

    //References
    private static CheckpointManager instance;

    public static CheckpointManager GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static int IsCheckpointNumber(Transform t)
    {
        for(int i = 0; i < trackCheckpoints.Length; i++)
        {
            if(trackCheckpoints[i] == t)
            {
                return i;
            }
        }
        return -1;
    }

}
