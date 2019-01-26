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
    public int score;
    public TextMeshProUGUI scoreText;
    //public TextMeshProUGUI timeText;
        //happiness meter
    public float happiness;
    public Image happinessFill;
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
        //GameManager.Instance.ScoreChanged += AddPoint;
       // Parent.Instance.HappinessChanged += UpdateHappiness;
    }
    void Update ()
    {
        PressEscape();
    }
    public void ElementState (RectTransform element, bool setstate) 
    {
        element.gameObject.SetActive(setstate);
    }
    public void AddPoint (int point)
    {
        score += point;
        scoreText.text = "Score: " + score;
    }
    public void UpdateHappiness (float happy)
    {
        if(happy > 0f && happy < 33f)
        {
            happinessFill.sprite = sadFace;
        }
        if(happy > 33f && happy < 66f)
        {
            happinessFill.sprite = neutralFace;
        }
        if(happy > 66f && happy < 100f)
        {
            happinessFill.sprite = happyFace;
        }
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
