using UnityEngine;
using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


public class PlayGames : MonoBehaviour
{
    public static PlayGames instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        try
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool success) => { });
        }
        catch (Exception exception)
        {
            Debug.Log(exception);
        }
    }

/*    public void AddScoreToLeaderboard()
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(playerScore, leaderboardID, success => { });
        }
    }

    public void ShowLeaderboard()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
    }*/

    public void ShowAchievements()
    {

        Social.ShowAchievementsUI();
        if (Social.localUser.authenticated)
        {
            Social.ShowAchievementsUI();
        }
    }

    public void UnlockAchievement(string id)
    {
        if (Social.localUser.authenticated)
        {
            
            Social.ReportProgress(id, 100f, success => { });
        }
    }

    public void SignInOrOut()
    {
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.SignOut();
        } else
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool success) => { });
        }
    }
}