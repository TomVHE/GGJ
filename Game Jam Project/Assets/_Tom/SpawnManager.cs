using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    public event Action<int> LivingLetters;


    public int LettersAlive
    {
        get => lettersAlive;
        set
        {
            lettersAlive = value;
            if(lettersAlive < 0)
            {
                lettersAlive = 0;
            }
            LivingLetters(lettersAlive);
        }
    }
    
    #if UNITY_EDITOR
    [Required, AssetsOnly]
    #endif    
    [SerializeField] private Transform letter;
    [SerializeField] private List<Transform> spawnPositions = new List<Transform>();
    [SerializeField] private int baseNumber = 0;
    [SerializeField] private int waveNumber = 5;
    [SerializeField] private float spawnTime = 1f;

    private int lettersAlive;
    private bool spawning;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (letter == null)
        {
            Debug.LogError("No letter object added!");
        }
    }

    private void Spawn()
    {
        if (lettersAlive == 0 && !spawning && !GameManager.Instance.gameOver)
        {
            StartCoroutine(Spawning());
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Spawn();
        }
    }

    private IEnumerator Spawning()
    {
        spawning = true;
        int spawnNumber = baseNumber + GameManager.Instance.CurrentWave * waveNumber;
        GameManager.Instance.LettersSpawned += spawnNumber;

        while (spawnNumber > 0)
        {
            spawnNumber--;
            LettersAlive++;
            Instantiate(letter, spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Count)].position, Quaternion.identity);
            yield return new WaitForSeconds(spawnTime);
        }
        spawning = false;
    }
}