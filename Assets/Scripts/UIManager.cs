using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Zenject;

public class UIManager : MonoBehaviour
{
    //settings
    public bool UIBatchTest = true;

    //references
    public static UIManager instance;
    private GameManager gameManager;
    public TextMeshProUGUI speedometer;
    public TextMeshProUGUI laptimeValue;
    public TextMeshProUGUI highscoreValue;
    public GameObject UITestImage;
    public GameObject UITestImage2;

    //internal
    private static int speedCached = -1;


    //zenject test
    [Inject]
    public void Construct(GameManager gameManager)
    {
        this.gameManager = gameManager;
        print("injected " + this.gameManager.name);
    }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        EventManager.OnLapFinished += ShowLapFinishedAnimation;
        EventManager.MainMenuButtonClickedEvent += ToggleMainMenu;
    }

    // Update is called once per frame
    void Update()
    {
        if (UIBatchTest)
        {
            UITestImage.SetActive(!UITestImage.activeSelf);
            UITestImage2.SetActive(!UITestImage2.activeSelf);
        }
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

    public void ShowLapFinishedAnimation(float lapTime)
    {
        print("lap animation! " + lapTime.ToString());
    }


    public void ToggleMainMenu()
    {
        print("toggled main menu");
    }

}
