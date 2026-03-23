using UnityEngine;
using TMPro;
using Vuforia;

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
        gameCountdown = true;
        countdown = 5;
    }

    private void Update()
    {
        // Run countdown before the game started
        if (gameCountdown)
        {
            countdown -= Time.deltaTime;

            if (countdown <= 0)
            {
                countdown = 0;
                gameCountdown = false;
                StartGame();
            }

            // Update countdown in HUD
            CountdownText.text = ((int)countdown).ToString();
        }
    }

    // Function to start the game after the countdown finished
    private void StartGame()
    {
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