using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timeRemaining = 300f; // Time in seconds (5 minutes)
    [SerializeField] private TextMeshProUGUI timerText; // Reference to the UI Text component for the timer
    [SerializeField] private TextMeshProUGUI anomalyCountText; // Reference to the UI Text component for the anomaly count
    [SerializeField] private string winScene; // Scene to load if the player wins
    [SerializeField] private string loseScene; // Scene to load if the player loses
    [SerializeField] private int requiredAnomaliesToSolve = 10; // Required anomalies to solve

    private bool timerIsRunning = true;

    private void Start()
    {
        // Reset anomaly count when the scene loads
        Anomaly.anomalyCount = 0;
        UpdateTimerDisplay(timeRemaining);
        UpdateAnomalyCountDisplay(Anomaly.anomalyCount);
    }

    private void Update()
    {
        if (!timerIsRunning) return;

        timeRemaining -= Time.deltaTime;
        UpdateTimerDisplay(timeRemaining);

        // Check if the anomaly count reaches the required number
        if (Anomaly.anomalyCount >= requiredAnomaliesToSolve)
        {
            timerIsRunning = false;
            SceneManager.LoadScene(winScene);
        }

        // Check if the time has run out
        if (timeRemaining <= 0)
        {
            timerIsRunning = false;
            SceneManager.LoadScene(Anomaly.anomalyCount >= requiredAnomaliesToSolve ? winScene : loseScene);
        }

        // Update the anomaly count display in case it changes during the game
        UpdateAnomalyCountDisplay(Anomaly.anomalyCount);
    }

    private void UpdateTimerDisplay(float timeToDisplay)
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void UpdateAnomalyCountDisplay(int solvedAnomalies)
    {
        // Only update the display if solved anomalies are less than or equal to required anomalies
        if (solvedAnomalies <= requiredAnomaliesToSolve)
        {
            anomalyCountText.text = $"{solvedAnomalies} / {requiredAnomaliesToSolve} Anomalies Solved";
        }
    }
}
