using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TextMeshProUGUI lastScoreText;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI highScoreText;

    private int lastScore = 0;
    private int currentPoints = 0;
    private int highScore = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Reattach UI whenever a new scene loads
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        Enemy.OnEnemyDied += OnEnemyDied;
        
        updatePoints();
        UpdateHighScore();
        UpdateLastScore();
    }

    void OnDestroy()
    {
        Enemy.OnEnemyDied -= OnEnemyDied;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Refind UI in the new scene
        FindUIReferences();

        // Refresh UI
        updatePoints();
        UpdateHighScore();
        UpdateLastScore();
    }

    void FindUIReferences()
    {
        // Only overwrite if found in this scene (so nulls are fine)
        TextMeshProUGUI tmp;

        tmp = FindTMP("Last Score");
        if (tmp != null) lastScoreText = tmp;

        tmp = FindTMP("Score");
        if (tmp != null) pointsText = tmp;

        tmp = FindTMP("Hi-Score");
        if (tmp != null) highScoreText = tmp;
    }

    TextMeshProUGUI FindTMP(string objectName)
    {
        GameObject obj = GameObject.Find(objectName);
        if (obj == null) return null;

        return obj.GetComponent<TextMeshProUGUI>();
    }

    void OnEnemyDied(int score)
    {
        currentPoints += score;

        updatePoints();
        UpdateHighScore();

        Debug.Log($"Killed enemy worth {score}");
    }

    void updatePoints()
    {
        if (pointsText == null) return;
        pointsText.text = $"Score\n{currentPoints.ToString("0000")}";
    }

    void UpdateHighScore()
    {
        highScore = Mathf.Max(highScore, currentPoints);

        if (highScoreText == null) return;
        highScoreText.text = $"Hi-Score\n{highScore.ToString("0000")}";
    }

    void UpdateLastScore()
    {
        lastScore = currentPoints;

        if (lastScoreText == null) return;
        lastScoreText.text = $"Score\n{lastScore.ToString("0000")}";
    }

    public void GotoMainMenu()
    {
        UpdateHighScore();
        UpdateLastScore();

        SceneManager.LoadScene("MainMenu");
    }

    public void GotoGame()
    {
        currentPoints = 0;

        SceneManager.LoadScene("Schmup");
    }

    public void GotoCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}