using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using System.Threading.Tasks;
using Unity.Services.Leaderboards;
using Newtonsoft.Json;
using Unity.Services.Leaderboards.Models;
using System.Collections.Generic;

public class UGSManager : MonoBehaviour
{
    public static  UGSManager Instance;

    async void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(Instance == null)
        {
            Instance = this;

            var options = new InitializationOptions();
            options.SetEnvironmentName("production");
            await UnityServices.InitializeAsync(options);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public async Task SignInAnonymouslyAsync()
    {
        AuthenticationService.Instance.ClearSessionToken();

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log("Signed in as guest");
        string randomName = "Player_" + Random.Range(1000, 9999);
        await AuthenticationService.Instance.UpdatePlayerNameAsync(randomName);
        Debug.Log($"Player Name: {AuthenticationService.Instance.PlayerName}");
    }

    public async void AddScore(string leaderboardId, int score)
    {
        Debug.Log($"Adding score {score} to leaderboard {leaderboardId}");
        var playerEntry = await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, score);
        Debug.Log(JsonConvert.SerializeObject(playerEntry));
    }
    
    public async void GetScores(string leaderboardId)
    {
        Debug.Log($"Getting scores from leaderboard {leaderboardId}");
        var scoreResponse = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
        List<LeaderboardEntry> entries = scoreResponse.Results;

        foreach(var entry in entries)
        {
            Debug.Log($"{entry.PlayerName}: {entry.Score}");
        }

        GameObject g = GameObject.Find("GameController");
        g.GetComponent<GameManager>().ShowLeaderboardUI(entries);
    }
}
