using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Services.Leaderboards.Models;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance { get { return instance; } }

    [SerializeField]
    private GameObject platformPrefab;
    public GameObject PlatformPrefab { get { return platformPrefab; } }

    private int platformCount = 1200;

    [SerializeField]
    private bool gameover = false;
    public bool gamepause = false;

    [SerializeField]
    private int score = 0;
    [SerializeField]
    private TMP_Text scoreText;
    private float lastPlayerY = 0f;
    
    [SerializeField]
    private UnityEngine.UI.Button PauseButton;
    [SerializeField]
    private UnityEngine.UI.Button ContinueButton;
    [SerializeField]
    private UnityEngine.UI.Button StartButton;
    [SerializeField]
    private UnityEngine.UI.Button MenuButton;
    [SerializeField]
    private UnityEngine.UI.Button HighScoreButton;
    [SerializeField]
    private UnityEngine.UI.Button BackButton;
    [SerializeField]
    private GameObject PauseMenu;
    [SerializeField]
    private GameObject MainMenu;
    [SerializeField]
    private GameObject GameOver;
    [SerializeField]
    private GameObject LeaderboardMenu;

    [Header("Leaderboard UI")]
    [SerializeField]
    private TMP_Text[] namesText;
    [SerializeField]
    private TMP_Text[] scoresText;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UGSManager.Instance.SignInAnonymouslyAsync();
        gamepause = true;
        Time.timeScale = 0;
        if (MainMenu != null)
        {
            MainMenu.SetActive(true);
        }

        if (StartButton != null)
        {
            StartButton.onClick.AddListener(StartGame);
        }

        // Spawn predetermined number of platforms
        Vector3 spawnPosition = new Vector3();
        for (int i = 0; i < platformCount; i++)
        {
            spawnPosition.y += Random.Range(8f, 12f);
            spawnPosition.x = Random.Range(-30f, 30f);
            Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        }

        if (PauseButton != null)
        {
            PauseButton.onClick.AddListener(TogglePause);
        }

        if (ContinueButton != null)
        {
            ContinueButton.onClick.AddListener(ResumeGame);
        }

        if (MenuButton != null)
        {
            MenuButton.onClick.AddListener(ResetGame);
            if (GameOver != null)
            {
                GameOver.SetActive(false);
            }
        }

        if (HighScoreButton != null)
        {
            HighScoreButton.onClick.AddListener(() => UGSManager.Instance.GetScores("HighScore"));
        }

        if (BackButton != null)
        {
            BackButton.onClick.AddListener(() => LeaderboardMenu.SetActive(false));
        }
    }

    void Update()
    {
        if (PlayerController.Instance != null)
        {
            float playerY = PlayerController.Instance.transform.position.y;
            float playerVelocityY = PlayerController.Instance.GetComponent<Rigidbody2D>().linearVelocity.y;

            if (playerVelocityY < -120f && !gameover)
            {
                gameOver();
            }

            if (playerY - lastPlayerY >= 10f)
            {
                score += 1;
                lastPlayerY = playerY;
                scoreText.text = score.ToString();
            }
        }
    }

    void gameOver()
    {
        gameover = true;
        if (GameOver != null)
        {
            GameOver.SetActive(true);
        }
        Time.timeScale = 0;
    }

    void TogglePause()
    {
        gamepause = !gamepause;
        if (PauseMenu != null)
        {
            PauseMenu.SetActive(gamepause);
        }
        Time.timeScale = gamepause ? 0 : 1;

        // Play ad when paused
        if (gamepause)
        {
            AdsManager adsManager = GetComponent<AdsManager>();
            if (adsManager != null)
            {
                adsManager.PlayAd();
            }
        }
    }

    void ResumeGame()
    {
        gamepause = false;
        if (PauseMenu != null)
        {
            PauseMenu.SetActive(false);
        }
        Time.timeScale = 1;
    }

    void StartGame()
    {
        gamepause = false;
        Time.timeScale = 1;
        if (MainMenu != null)
        {
            MainMenu.SetActive(false);
        }
    }

    public void ShowLeaderboardUI(List<LeaderboardEntry> entries)
    {
        LeaderboardMenu.SetActive(true);
        for(int i = 0; i < scoresText.Length; i++)
        {
            if(entries.Count <= i)
            {
                scoresText[i].text = "";
                namesText[i].text = "";
            }
            else
            {
                scoresText[i].text = entries[i].Score.ToString();
                namesText[i].text = entries[i].PlayerName;
            }
        }

    }

    void ResetGame()
    {
        SendScoreToLeaderboard();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SendScoreToLeaderboard()
    {
        UGSManager.Instance.AddScore("HighScore", score);
    }

    public void LoadLeaderboard()
    {
        UGSManager.Instance.GetScores("HighScore");
    }

    private void SetLeaderboardMenuActive(bool active)
    {
        if (LeaderboardMenu != null)
        {
            LeaderboardMenu.SetActive(active);
        }
    }
}
