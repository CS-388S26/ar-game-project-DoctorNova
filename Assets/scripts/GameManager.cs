using UnityEngine;
using TMPro;
using Vuforia;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float currentHeight = 0;
    public float highScore = 0;

    public TextMeshProUGUI heightText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI CountdownText;

    public bool gameStarted = false;
    public bool gameOver = false;
    public bool gameCountdown = false;

    public BlockSpawner spawner;

    private float countdown = 3;

    // PlaneFinder reference
    public AnchorInputListenerBehaviour anchorInputListener;

    void Awake()
    {
        Instance = this;
        highScore = PlayerPrefs.GetFloat("HighScore", 0);
        UpdateUI();
    }

    // Start the game with a countdown for the player to get ready
    public void StartCountdown()
    {
        if (gameCountdown || gameStarted)
        {
            return;
        }

        gameCountdown = true;
        countdown = 6;
    }

    private void Update()
    {
        // Run countdown before the game started
        if (gameCountdown)
        {
            countdown -= Time.deltaTime;

            // Update countdown in HUD
            CountdownText.text = ((int)countdown).ToString();

            if (countdown <= 0)
            {
                CountdownText.text = "";
                countdown = 0;
                gameCountdown = false;
                StartGame();
            }

        }
    }

    // Function to start the game after the countdown finished
    private void StartGame()
    {
        spawner.SpawnBlock();
        gameStarted = true;
        gameOver = false;
        currentHeight = 0;
        UpdateUI();

        // Disable the PlaneFinder repositioning behavior during the game
        if (anchorInputListener != null)
        {
            anchorInputListener.enabled = false;  // Disable repositioning
        }
    }

    // Add a block to the current score and update HUD
    public void AddBlock(float yPosition)
    {
        if (gameOver) return;

        yPosition = yPosition - spawner.platform.transform.position.y;

        if (yPosition > currentHeight)
        {
            currentHeight = yPosition;
        }

        if (currentHeight > highScore)
        {
            highScore = currentHeight;
            PlayerPrefs.SetFloat("HighScore", highScore);
        }

        UpdateUI();
    }

    // End the game if the player lost
    public void EndGame()
    {
        CountdownText.text = "Game over";

        spawner.Reset();
        gameOver = true;
        gameStarted = false;

        // Re-enable the PlaneFinder's repositioning when the game ends
        if (anchorInputListener != null)
        {
            anchorInputListener.enabled = true;  // Re-enable repositioning
        }

        StartCountdown();
    }

    // Update the HUD
    void UpdateUI()
    {
        if (heightText != null)
            heightText.text = "Score: " + Mathf.RoundToInt(currentHeight * 100);

        if (highScoreText != null)
            highScoreText.text = "Highscore: " + Mathf.RoundToInt(highScore * 100);
    }
}