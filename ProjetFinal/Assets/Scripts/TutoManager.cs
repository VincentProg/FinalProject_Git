using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutoManager : MonoBehaviour
{

    bool isStepDone = false;
    bool wait = false;
    HexCell tileToClick;

    int oldroundCount = 0;
    int step = 0;

    [SerializeField]
    HeroController hero1, hero2;

    public Transform canvasHero1;
    public Transform canvasHero2;
    HexCell tileAimed = null;

    public GameObject arrow;

    int nbrPassTurnHero1 = 0;
    int nbrPassTurnHero2 = 0;

    private void Start()
    {
        hero1.canPlay = false;
        hero2.canPlay = false;
        TilesManager.instance.ClearTiles(false);


        ManipulateCanvas(canvasHero1, false, false, 4);
        ManipulateCanvas(canvasHero2, false, false, 4);

    }

    private void Update()
    {

        if (CombatSystem.instance.nbrRound != oldroundCount)
        {
            oldroundCount = CombatSystem.instance.nbrRound;
        }

        if(oldroundCount == 1)
        {
            if (step == 0)
            {
                // Dialogue et disparition dialogue
                if (!isStepDone)
                {
                    DialogueRobot.instance.RobotSpeak("Salut cher ami, bienvenu dans ta mission d'éradication d'aliens");
                    isStepDone = true;
                }
                else if (TouchScreen() && DialogueRobot.instance.textEnded)
                {
                    StartCoroutine(DialogueRobot.instance.iDisappear());
                    step++;
                    isStepDone = false;
                }
                
            } else if(step == 1)
            {
                // Selection 1 case
                if (!isStepDone && !DialogueRobot.instance.isActive)
                {
                    DialogueRobot.instance.RobotSpeak("Appui sur une case sélectionnée verte pour y déplacer ton héro.");
                    
                    HexCoordinates coordinates = new HexCoordinates(hero1.myTile.coordinates.X + 1, hero1.myTile.coordinates.Z);
                    TilesManager.instance.mapTiles.TryGetValue(coordinates, out tileAimed);
                    tileAimed.SelectCell(HexCell.SELECTION_TYPE.MOVEMENT);
                    isStepDone = true;
                }
                
                else if (TouchTile(tileAimed) && DialogueRobot.instance.textEnded)
                {
                    
                    StartCoroutine(DialogueRobot.instance.iDisappear());
                    hero1.Move(tileAimed);
                    step++;
                    isStepDone = false;
                }
            }

            if(step == 2)
            {

                // Fin du tour
                if (!DialogueRobot.instance.isActive)
                {
                    StartCoroutine(RobotSpeakDelay("Parfait. Ton marine n'a désormais plus aucune action possible. Appuie sur ce bouton pour finir ton tour ", 0.5f));
                    ManipulateCanvas(canvasHero1, true, false);
                    arrow.SetActive(true);
                    step++;
                }
            }

            if(step == 4)
            {
                if (!DialogueRobot.instance.isActive)
                {
                    DialogueRobot.instance.RobotSpeak("Déplace à présent ton second héro sur la case verte.");
                    step++;
                }
            }

            if(step == 5)
            {
                if (TouchTile(tileAimed) && DialogueRobot.instance.textEnded)
                {
                    hero2.Move(tileAimed);
                   
                    step++;
                    StartCoroutine(DialogueRobot.instance.iDisappear());
                }
            }

            if(step == 6)
            {
                if (!DialogueRobot.instance.isActive)
                {
                    DialogueRobot.instance.RobotSpeak("Contrairement au marine, le cowboy peut effectuer 1 action + 1 déplacement durant son tour ! Profites en pour attaquer l'ennemi situé derrière les rochers");
                    ManipulateCanvas(canvasHero2, false, true,1);
                    step++;
                }

            }
            if(step == 7)
            {
                bool isTileGet = false;
                if (!isTileGet)
                {
                    HexCoordinates coordinates = new HexCoordinates(3, 5);
                    TilesManager.instance.mapTiles.TryGetValue(coordinates, out tileAimed);
                    isTileGet = true;
                }
                TouchAimedTile(tileAimed, hero2);
            }
            if(step == 8)
            {
                if (!DialogueRobot.instance.isActive)
                {
                    DialogueRobot.instance.RobotSpeak("Le cowboy n'a plus aucune action possible, il peut finir son tour.");;
                    step++;
                }
            }


        }


    }

    bool TouchScreen()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) return true;
        return false;
    }

    bool TouchTile(HexCell tile)
    {
        
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
            RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
            if (hitInformation)
            {
                if (hitInformation.transform.GetComponent<HexCell>() != null)
                {
                    if (hitInformation.transform.GetComponent<HexCell>() == tile) return true;
                }
            }
        }
        return false;
    }

    void TouchAimedTile(HexCell aimed, HeroController hero)
    {

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            

            Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
            RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
            if (hitInformation)
            {
                if (hitInformation.transform.GetComponent<HexCell>() != null)
                {
                    HexCell tileScript = hitInformation.transform.GetComponent<HexCell>();
                    if (tileScript.selectionType == HexCell.SELECTION_TYPE.AIM)
                    {
                        TilesManager.instance.ClearTiles(true);
                        tileScript.SelectCell(HexCell.SELECTION_TYPE.ORIGIN_IMPACT);
                    } else if (tileScript.selectionType == HexCell.SELECTION_TYPE.ORIGIN_IMPACT)
                    {
                        if(tileScript == aimed)
                        {
                            Hero_AttacksManager.instance.LaunchAttack(hero);
                            ManipulateCanvas(canvasHero2, true, false, 1);
                            StartCoroutine(DialogueRobot.instance.iDisappear());
                            step++;
                        }
                    }
                }
            }
        }
    }

    public void TouchButtonPassTurnHero1()
    {
        if (nbrPassTurnHero1 == 0)
        {
            arrow.SetActive(false);
            StartCoroutine(DialogueRobot.instance.iDisappear());


            HexCoordinates coordinates = new HexCoordinates(hero2.myTile.coordinates.X + 1, hero2.myTile.coordinates.Z);
            TilesManager.instance.mapTiles.TryGetValue(coordinates, out tileAimed);
            tileAimed.SelectCell(HexCell.SELECTION_TYPE.MOVEMENT);
            step++;

        }
    }

    public void TouchButtonPassTurnHero2()
    {
        if (nbrPassTurnHero2 == 0)
        {
            StartCoroutine(DialogueRobot.instance.iDisappear());



        }
    }

    IEnumerator RobotSpeakDelay(string sentence, float delay)
    {
        yield return new WaitForSeconds(delay);
        DialogueRobot.instance.RobotSpeak(sentence);
    }

    void ManipulateCanvas(Transform canvas, bool isPassTurnInteractable, bool isActionsInteractable = false, int attacks = 0)
    {
        if (attacks != 0)
        {
            if (attacks < 4)
            {
                canvas.GetChild(0).GetChild(attacks - 1).GetComponent<Button>().interactable = isActionsInteractable;
                if (!isActionsInteractable)
                     canvas.GetChild(0).GetChild(attacks - 1).GetComponent<Image>().color = Color.gray;
                else canvas.GetChild(0).GetChild(attacks - 1).GetComponent<Image>().color = Color.white;
            }
            else
            foreach (Transform child in canvas.GetChild(0))
            {
                child.GetComponent<Button>().interactable = isActionsInteractable;
                if (!isActionsInteractable)
                    child.GetComponent<Image>().color = Color.gray;
                else child.GetComponent<Image>().color = Color.white;
            }
        } 
        
        canvas.GetChild(1).GetComponent<Button>().interactable = isPassTurnInteractable;
        if(!isPassTurnInteractable)
        canvas.GetChild(1).GetComponent<Image>().color = Color.gray;
        else canvas.GetChild(1).GetComponent<Image>().color = Color.white;
    }
}
