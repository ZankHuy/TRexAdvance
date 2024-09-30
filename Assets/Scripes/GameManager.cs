using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float gameSpeed { get; private set; }

    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiscoreText;
    public Button retryButton;

    public GameObject boss; // Reference to the moving object prefab
    private Player player;
    private Spawner spawner;
    public Slider slider;
    private Bossphase bossPhase;  // Reference to Bossphase component
    private List<GameObject> spawnedBossObjects = new List<GameObject>(); // List to track all spawned moving objects

    public float specialEventTimer = 15f; // 30-second timer for special event
    private bool isSpecialEventActive = false;

    private float score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        spawner = FindObjectOfType<Spawner>();
        bossPhase = FindObjectOfType<Bossphase>(); // Get Bossphase component
        NewGame();
    }

    public void NewGame()
    {
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();
        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        gameSpeed = initialGameSpeed;
        score = 0f;
        enabled = true;

        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
        bossPhase.gameObject.SetActive(false); // Disable boss phase at start
        slider.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        UpdateHiscore();
        specialEventTimer = 15f; // Reset the special event timer
        isSpecialEventActive = false; // Ensure the event is not active at the start
    }

    public void GameOver()
    {
        gameSpeed = 0;
        enabled = false;

        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
        bossPhase.gameObject.SetActive(false); // Disable boss phase at start
        slider.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
        EndSpecialEvent();
        UpdateHiscore();
    }

    private void Update()
    {
        gameSpeed += gameSpeedIncrease * Time.deltaTime;
        score += gameSpeed * Time.deltaTime;
        scoreText.text = Mathf.FloorToInt(score).ToString("D5");

        // Handle special event
        if (!isSpecialEventActive)
        {
            specialEventTimer -= Time.deltaTime;
            if (specialEventTimer <= 0)
            {
                StartSpecialEvent();
            }
        }

        if (isSpecialEventActive && slider.value >= 1f)
        {
            EndSpecialEvent();
        }
    }

    private void UpdateHiscore()
    {
        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if (score > hiscore)
        {
            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }

        hiscoreText.text = Mathf.FloorToInt(hiscore).ToString("D5");
    }

    private void StartSpecialEvent()
    {
        slider.value = 0;
        isSpecialEventActive = true;
        bossPhase.gameObject.SetActive(true); // Activate boss phase
        slider.gameObject.SetActive(true);
        // Spawn the moving object at the start of the special event
        SpawnBossObject();
    }

    private void SpawnBossObject()
    {
        // Set the spawn position, typically off-screen on the right side
        Vector3 spawnPosition = new Vector3(12f, 1.8f); // Adjust spawn Y-position randomly
        GameObject movingObject = Instantiate(boss, spawnPosition, Quaternion.identity); // Use 'boss' prefab to spawn the object

        // Add the spawned object to the list
        spawnedBossObjects.Add(movingObject);
    }

    public void EndSpecialEvent()
    {
        isSpecialEventActive = false;
        bossPhase.gameObject.SetActive(false); // Deactivate boss phase
        slider.gameObject.SetActive(false);

        // Destroy all spawned moving objects
        foreach (var movingObject in spawnedBossObjects)
        {
            if (movingObject != null)
            {
                Destroy(movingObject);
            }
        }

        // Clear the list after destruction
        spawnedBossObjects.Clear();

        // Reset the 30-second timer for the next special event
        specialEventTimer = 15f;
    }
}
