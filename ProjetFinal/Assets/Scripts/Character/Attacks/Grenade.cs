using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grenade : MonoBehaviour
{

    public enum TYPE_GRENADE {EXPLOSE, FLASH};
    public TYPE_GRENADE type;

    public int delay = 1;
    public int range;
    public int damagesExplose;
    public int nbrTurnSkipFlash;


    public bool isFriendlyFire;

    [HideInInspector]
    public HexCell myTile;

    [HideInInspector]
    public HeroController hero;


    public void StartTurn()
    {
        delay--;
        if(delay < 0)
        {
            Explode();
        }
    }


    private void Explode()
    {
        AudioManager.instance.Play(Hero_AttacksManager.instance.attack.nomDuSon);
        List<HexCell> listTiles = new List<HexCell>();
        listTiles = TilesManager.instance.GetRange(myTile.coordinates, range, true, false);

        switch (type)
        {

            case TYPE_GRENADE.EXPLOSE:
                bool playedFirst = false;
                foreach (HexCell tile in listTiles)
                {

                    GameObject particle = Instantiate(hero.grenadeExplosionParticle, tile.transform);
                    particle.transform.localScale = new Vector3(.2f, .2f, .2f);

                    if (playedFirst)
                    {
                        particle.GetComponent<AudioSource>().mute = true;
                    } else
                    {
                        playedFirst = true;
                    }

                    if (isFriendlyFire && tile.hero != null)
                    {
                        tile.hero.TakeDamages(damagesExplose, "grenade", "explosion");
                    } else if(tile.enemy != null)
                    {
                        tile.enemy.TakeDamages(damagesExplose, "grenade", "explosion");
                    }

                }

                break;

            case TYPE_GRENADE.FLASH:

                foreach (HexCell tile in listTiles)
                {
                    GameObject particle = Instantiate(hero.flashParticle, tile.transform);
                    particle.transform.localScale = new Vector3(.4f, .4f, .4f);

                    var main = particle.GetComponent<ParticleSystem>().main;
                    main.simulationSpeed = 0.5f;

                    if (!isFriendlyFire && tile.hero != null)
                    {
                        tile.hero.SkipTurns(nbrTurnSkipFlash);
                    }
                    else if (tile.enemy != null)
                    {
                        tile.enemy.SkipTurns(nbrTurnSkipFlash);
                        tile.enemy.isStun = true;
                        AchievementsManager.TriggerAchievement("CgkImpif4cQQEAIQBQ", myTile.coordinates);
                    }
                }

                break;

        }

        Death();

    }

    private void Death()
    {
        hero.grenades.Remove(this);
        Destroy(gameObject);
    }
}
