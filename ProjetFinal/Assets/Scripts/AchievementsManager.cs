using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsManager
{
    /*    // Finir le premier niveau
            CgkImpif4cQQEAIQAg //

            // Finir le jeu
            CgkImpif4cQQEAIQAw

            // Vaincre 3 ennemis d’affilés avec le cowboy
            CgkImpif4cQQEAIQBA //

            // Toucher 3 ennemis d’affilés avec la flash
            CgkImpif4cQQEAIQBQ //

            // Tuer 3 ennemis d’affilés avec la charge explosive
            CgkImpif4cQQEAIQBg //

            // Utiliser les rocket boots lorsque deux ennemis sont à côté du cowboy
            CgkImpif4cQQEAIQBw

            // Terminer un niveau sans tuer personne avec l’un des deux héros
            CgkImpif4cQQEAIQCA

            // Finir un niveau sans tuer personne
            CgkImpif4cQQEAIQCQ

            // Tuer au moins 4 ennemis stuns par le grenade flash avec la grenade du soldat
            CgkImpif4cQQEAIQCg

            // Tuer son allié avec une grenade
            CgkImpif4cQQEAIQCw

            // Passer son premier tour sur les deux personnage
            CgkImpif4cQQEAIQDA

            // Finir un niveau avec seulement 1pv restant sur les 2 persos
            CgkImpif4cQQEAIQDQ

            // Ne pas bouger pendant 3 tours avec le soldat
            CgkImpif4cQQEAIQDg

            // Réussir à tuer 5 ennemis ou + en un seul tour global
            CgkImpif4cQQEAIQDw
    */


    public static Dictionary<string, int> achievement_progress = new Dictionary<string, int>();
    public static Dictionary<string, int> achievement_lastupdate = new Dictionary<string, int>();

    public static bool IsInteresting(string id)
    {
        return achievement_progress.ContainsKey(id);
    }


    public static void TriggerAchievement(string id)
    {

        if (achievement_progress.ContainsKey(id))
        {
            if (id.Equals("CgkImpif4cQQEAIQCQ"))
            {
                IncrementProgress(id);
            }
            if (id.Equals("CgkImpif4cQQEAIQDQ"))
            {

                bool pvOk = true;
                for (int i = 0; i < CombatSystem.instance.heros.Count; i++)
                {
                    if (CombatSystem.instance.heros[i].health != 1)
                    {
                        pvOk = false;
                    }
                }
                if (pvOk)
                    IncrementProgress(id);
            }
            else if (id.Equals("CgkImpif4cQQEAIQAg"))
            {
                IncrementProgress(id);

            }
            else
            {
                int value;
                achievement_lastupdate.TryGetValue(id, out value);

                if (CombatSystem.instance.nbrRound == value + 1)
                {
                    IncrementProgress(id);
                }
                else if (CombatSystem.instance.nbrRound != value)
                {
                    ResetProgress(id);
                    IncrementProgress(id);
                }
            }
        }
    }

    public static void TriggerAchievement(string id, HexCoordinates coords)
    {
        if (id.Equals("CgkImpif4cQQEAIQBw"))
        {
            if (achievement_progress.ContainsKey(id))
            {
                List<HexCell> list = TilesManager.instance.GetRadius(coords, 1, true, true, true);
                foreach (var item in list)
                {
                    if(item.enemy != null)
                    {
                        Debug.Log("yes");
                    } else
                    {
                        Debug.Log("nop");

                    }
                }
            }
        }
    }


    public static void IncrementProgress(string id)
    {
        if (achievement_progress.ContainsKey(id))
        {
            int score;
            achievement_progress.TryGetValue(id, out score);
            score++;
            achievement_progress.Remove(id);
            achievement_progress.Add(id, score);


            achievement_lastupdate.Remove(id);
            achievement_lastupdate.Add(id, CombatSystem.instance.nbrRound);


            CheckAchievementStatus(id);
        }
    }

    public static void ResetProgress(string id)
    {

        if (achievement_progress.ContainsKey(id))
        {
            achievement_progress.Remove(id);
            achievement_progress.Add(id, 0);

            CheckAchievementStatus(id);

        }
    }

    public static void CheckAchievementStatus(string id)
    {

        if (achievement_progress.ContainsKey(id))
        {
            int score;
            achievement_progress.TryGetValue(id, out score);
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
                    Debug.LogWarning("CgkImpif4cQQEAIQBA " + score);
                    if (score >= 2)
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
                    if (score == 0)
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
                    if (score >= 1)
                    {
                        Debug.LogWarning(id + " ok");
                        PlayGames.instance.UnlockAchievement(id);
                    }
                    break;

                case "CgkImpif4cQQEAIQDg":
                    Debug.LogWarning("CgkImpif4cQQEAIQDg " + score);
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
