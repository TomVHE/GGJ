using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //testing
    //public bool testbool;
    //public RectTransform testRect;
    //public bool go;


    //main menu
    public RectTransform mainMenu; //play, settings, quit game, credits
    

    //settings
    //public SettingsManager settingsManager;    
        
    //ingame
    public RectTransform ingame;
    public int killed;
    public int spawned;
    public TextMeshProUGUI scoreText;
    //public TextMeshProUGUI timeText;
        //happiness meter
    public float happiness;
    public Image happinessFill;
    public Image happinessExpression;
    public TextMeshProUGUI happinessText;
    public Sprite happyFace, neutralFace, sadFace;
        //paused
    public bool paused;
    public RectTransform pausedMenu;

    //game over
    public RectTransform gameOver; //has mainmenu button and replay button

    public Gradient gradient;



    void Start () 
    {
        //subscribe to score and happiness
        GameManager.Instance.Killed += AddKills;
        GameManager.Instance.Spawned += AddSpawned;
        Parent.Instance.HappinessChanged += UpdateHappiness;

    }
    void Update ()
    {
        PressEscape();
    }
    public void ElementState (RectTransform element, bool setstate) 
    {
        element.gameObject.SetActive(setstate);
    }
    public void AddSpawned (int point)
    {
        spawned = point;    //CHANGE SCORE
        scoreText.text = killed + " <size=125%>/" + spawned;
    }
    public void AddKills (int point)
    {
        killed += point;
        scoreText.text = killed + " <size=125%>/" + spawned;
        // scoreText.text = 12 
    }
    public void UpdateHappiness (float happy)
    {
        if(happy > 0f && happy < 33f)
        {
            happinessExpression.sprite = sadFace;
        }
        if(happy > 33f && happy < 66f)
        {
            happinessExpression.sprite = neutralFace;
        }
        if(happy > 66f && happy < 100f)
        {
            happinessExpression.sprite = happyFace;
        }
        //color doesnt change and number doesnt either
        Debug.LogError("color doesnt change and number doesnt either");
        happinessFill.fillAmount = happy/100f;
        happinessFill.color = gradient.Evaluate(happiness/100f);
    }
    public void QuitGame ()
    {
        Application.Quit();   
    }
    public void PressEscape ()
    {
        if(Input.GetButtonDown("Escape"))
        {
            PausedCheck();
        }
    }
    public void PausedCheck ()
    {
        paused = !paused;
        pausedMenu.gameObject.SetActive(paused);
        if(paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    /* 
    public void UpdateTimer (float time)
    {
        timeText.text = "Time: " + time;
    }*/
}
