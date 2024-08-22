using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

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
        UpdateTimerDisplay(timeRemaining);
        UpdateAnomalyCountDisplay(Anomaly.anomalyCount);
    }

    private void Update()
    {
        if (!timerIsRunning) return;

        timeRemaining -= Time.deltaTime;
        UpdateTimerDisplay(timeRemaining);

        if (timeRemaining <= 0)
        {
            timerIsRunning = false;
            SceneManager.LoadScene(Anomaly.anomalyCount >= requiredAnomaliesToSolve ? winScene : loseScene);
        }

        // Call this method whenever an anomaly is solved.
        // For example, you could call it from another script or event when the anomaly count increases.
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
