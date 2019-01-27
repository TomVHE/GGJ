
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    public int LettersAlive
    {
        get => lettersAlive;
        set
        {
            lettersAlive = value;
            //GameManager.Instance.LettersSpawned--;
        }
    }
    
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
        if (lettersAlive == 0 && !spawning)
        {
            StartCoroutine(Spawning());
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
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
            lettersAlive++;
            Instantiate(letter, spawnPositions[Random.Range(0, spawnPositions.Count)].position, Quaternion.identity);
            yield return new WaitForSeconds(spawnTime);
        }
        spawning = false;
    }
}
