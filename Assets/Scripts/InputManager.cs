using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static bool steerLeftPressed = false;
    public static bool steerRightPressed = false;

    // Update is called once per frame
    void Update()
    {

    }

    public void SteerLeftButtonPressed()
    {
        steerLeftPressed = true;
    }

    public void SteerRightButtonPressed()
    {
        steerRightPressed = true;
    }

    public void SteerLeftButtonReleased()
    {
        steerLeftPressed = false;
    }

    public void SteerRightButtonReleased()
    {
        steerRightPressed = false;
    }

    public void LoadTrackButtonClicked(int trackIndex)
    {
        GameManager.GetInstance().LoadTrack(trackIndex);
    }
}
