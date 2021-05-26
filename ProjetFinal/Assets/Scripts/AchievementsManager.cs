using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsManager
{
    public static bool CgkImpif4cQQEAIQBA_temp_ok = false;
    public static bool CgkImpif4cQQEAIQCQ_temp_ok = true;

    public static bool CgkImpif4cQQEAIQDg_temp_ok = true;

    private static Dictionary<string, int> achieviement_status = new Dictionary<string, int>()
    {
        // Finir le premier niveau
        { "CgkImpif4cQQEAIQAg", 0 },

        // Finir le jeu
        { "CgkImpif4cQQEAIQAw", 0 },

        // Vaincre 3 ennemis d’affilés avec le cowboy
        { "CgkImpif4cQQEAIQBA", 0 },

        // Toucher 3 ennemis d’affilés avec la flash
        { "CgkImpif4cQQEAIQBQ", 0 },

        // Tuer 3 ennemis d’affilés avec la charge explosive
        { "CgkImpif4cQQEAIQBg", 0 },

        // Utiliser les rocket boots lorsque deux ennemis sont à côté du cowboy
        { "CgkImpif4cQQEAIQBw", 0 },

        // Terminer un niveau sans tuer personne avec l’un des deux héros
        { "CgkImpif4cQQEAIQCA", 0 },

        // Finir un niveau sans tuer personne
        { "CgkImpif4cQQEAIQCQ", 0 },

        // Tuer au moins 4 ennemis stuns par le grenade flash avec la grenade du soldat
        { "CgkImpif4cQQEAIQCg", 0 },

        // Tuer son allié avec une grenade
        { "CgkImpif4cQQEAIQCw", 0 },

        // Passer son premier tour sur les deux personnage
        { "CgkImpif4cQQEAIQDA", 0 },

        // Finir un niveau avec seulement 1pv restant sur les 2 persos
        { "CgkImpif4cQQEAIQDQ", 0 },

        // Ne pas bouger pendant 3 tours avec le soldat
        { "CgkImpif4cQQEAIQDg", 0 },

        // Réussir à tuer 5 ennemis ou + en un seul tour global
        { "CgkImpif4cQQEAIQDw", 0 },

    };


    public static void IncrementProgress(string id)
    {
        if (achieviement_status.ContainsKey(id))
        {
            int score;
            achieviement_status.TryGetValue(id, out score);
            score++;
            achieviement_status.Remove(id);
            achieviement_status.Add(id, score);

            CheckAchievementStatus(id);
        }
    }

    public static void ResetProgress(string id)
    {

        if (achieviement_status.ContainsKey(id))
        {
            achieviement_status.Remove(id);
            achieviement_status.Add(id, 0);

            CheckAchievementStatus(id);

        }
    }

    public static void CheckAchievementStatus(string id)
    {

        if (achieviement_status.ContainsKey(id))
        {
            int score;
            achieviement_status.TryGetValue(id, out score);
            Debug.Log(id + " " + score);
            switch (id)
            {
                case "CgkImpif4cQQEAIQAg":
                    if (score >= 1)
                    {
                        Debug.LogWarning(id + " ok");
                        PlayGames.instance.UnlockAchievement(id);
                    }
                    break;

                case "CgkImpif4cQQEAIQAw":
                    if (score >= 1)
                    {
                        Debug.LogWarning(id + " ok");
                        PlayGames.instance.UnlockAchievement(id);
                    }
                    break;

                case "CgkImpif4cQQEAIQBA":
                    if (score >= 3)
                    {
                        Debug.LogWarning(id + " ok");
                        PlayGames.instance.UnlockAchievement(id);
                    }
                    break;

                case "CgkImpif4cQQEAIQBQ":
                    if (score >= 3)
                    {
                        Debug.LogWarning(id + " ok");
                        PlayGames.instance.UnlockAchievement(id);
                    }
                    break;

                case "CgkImpif4cQQEAIQBg":
                    if (score >= 3)
                    {
                        Debug.LogWarning(id + " ok");
                        PlayGames.instance.UnlockAchievement(id);
                    }
                    break;

                case "CgkImpif4cQQEAIQBw":
                    if (score >= 1)
                    {
                        Debug.LogWarning(id + " ok");
                        PlayGames.instance.UnlockAchievement(id);
                    }
                    break;

                case "CgkImpif4cQQEAIQCA":
                    if (score >= 1)
                    {
                        Debug.LogWarning(id + " ok");
                        PlayGames.instance.UnlockAchievement(id);
                    }
                    break;

                case "CgkImpif4cQQEAIQCQ":
                    if (score >= 1)
                    {
                        Debug.LogWarning(id + " ok");
                        PlayGames.instance.UnlockAchievement(id);

                    }
                    break;

                case "CgkImpif4cQQEAIQCg":
                    if (score >= 4)
                    {
                        Debug.LogWarning(id + " ok");
                        PlayGames.instance.UnlockAchievement(id);
                    }
                    break;

                case "CgkImpif4cQQEAIQCw":
                    if (score >= 1)
                    {
                        Debug.LogWarning(id + " ok");
                        PlayGames.instance.UnlockAchievement(id);
                    }
                    break;

                case "CgkImpif4cQQEAIQDA":
                    if (score >= 2)
                    {
                        Debug.LogWarning(id + " ok");
                        PlayGames.instance.UnlockAchievement(id);
                    }
                    break;

                case "CgkImpif4cQQEAIQDQ":
                    if (score >= 2)
                    {
                        Debug.LogWarning(id + " ok");
                        PlayGames.instance.UnlockAchievement(id);
                    }
                    break;

                case "CgkImpif4cQQEAIQDg":
                    if (score >= 3)
                    {
                        Debug.LogWarning(id + " ok");
                        PlayGames.instance.UnlockAchievement(id);
                    }
                    break;

                case "CgkImpif4cQQEAIQDw":
                    if (score >= 5)
                    {
                        Debug.LogWarning(id + " ok");
                        PlayGames.instance.UnlockAchievement(id);
                    }
                    break;


                default:
                    break;
            }
        }
    }


}
