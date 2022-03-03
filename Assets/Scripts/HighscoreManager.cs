using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreManager : MonoBehaviour
{
    private const string highscorePath = "trackHighscore";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetHighscore(float time, int trackIndex)
    {
        PlayerPrefs.SetFloat(Application.persistentDataPath + highscorePath + trackIndex.ToString(), time);
        UIManager.SetHighscoreValue(time);
    }

    public static float GetHighscore(int trackIndex)
    {
        return PlayerPrefs.GetFloat(Application.persistentDataPath + highscorePath + trackIndex.ToString(), -1f);
    }

    public static void ReportLapTime(float time, int trackIndex)
    {
        if (time > 0f && (time < GetHighscore(trackIndex) || GetHighscore(trackIndex) < 0f))
        {
            SetHighscore(time, trackIndex);
        }
    }

}
