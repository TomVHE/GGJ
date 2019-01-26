using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //testing
    public bool testbool;
    public RectTransform testRect;
    public bool go;


    //main menu
    public RectTransform mainMenu; //play, settings, quit game, credits
    

    //settings
    //public SettingsManager settingsManager;    
        
    //ingame
    public RectTransform ingame;
    public int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
        //happiness meter
    public float happiness;
    public Slider happinessSlider;
    public Color happyColor, neutralColor, sadColor;
    public Image happyFace, neutralFace, sadFace;

    //game over
    public RectTransform gameOver; //has mainmenu button and replay button




    void Start () 
    {
        //subscribe to score and happiness
        //GameManager.Instance.ScoreChanged += AddPoint;
        //Parent.Instance.HappinessChanged += UpdateHappiness;
    }
    // Update is called once per frame
    void Update ()
    {
        if(go)
        {
            ElementState(testRect, testbool);
            go = false;
        }
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
        //update the happiness
        //set face
        //set slider
        
    }
    /* 
    public void UpdateTimer (float time)
    {
        timeText.text = "Time: " + time;
    }*/
}
