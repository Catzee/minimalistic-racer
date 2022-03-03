using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void MainMenuButtonClicked();
    public static event MainMenuButtonClicked MainMenuButtonClickedEvent;
    public delegate void OnLapFinishedEvent(float lapTime);
    public static event OnLapFinishedEvent OnLapFinished;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MainMenuButtonClickedEvent();
        }
    }

    public static void InvokeLapFinishedEvent(float lapTime)
    {
        OnLapFinished(lapTime);
    }

}
