using UnityEngine;
using UnityEngine.UI;
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
        if(!Social.localUser.authenticated)
            SignInOrOut();
    }

    public void initAchievements()
    {
        if (Social.localUser.authenticated)
        {
            AchievementsManager.achievement_progress.Clear();
            AchievementsManager.achievement_lastupdate.Clear();

            Social.LoadAchievements(achievements =>
            {
                foreach (var item in achievements)
                {
                    if (!item.completed)
                    {
                        AchievementsManager.achievement_progress.Add(item.id, 0);
                        AchievementsManager.achievement_lastupdate.Add(item.id, -1);
                    }
                }
            });
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
        else
        {
            SignInOrOut();
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
        //Debug.LogError("login");
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.SignOut();
        } else
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();

            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();

            Social.localUser.Authenticate((bool success) => {
                if (success)
                {
                    initAchievements();
                }
            });
        }
    }
}