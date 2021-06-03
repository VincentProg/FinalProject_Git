using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutoManager : MonoBehaviour
{

    bool isStepDone = false;
    bool wait = false;
    HexCell tileToClick;

    int step = 0;

    [SerializeField]
    HeroController hero1, hero2;

    public Transform canvasHero1;
    public Transform canvasHero2;
    HexCell tileAimed = null;

    public GameObject arrow;

    int nbrPassTurnHero1 = 0;
    int nbrPassTurnHero2 = 0;

    bool boolean = false;

    public GameObject lastOrder;

    private void Start()
    {
        hero1.canPlay = false;
        hero2.canPlay = false;
        TilesManager.instance.ClearTiles(false);
        DialogueRobot.instance.isTuto = true;

        ManipulateCanvas(canvasHero1, false, false, 4);
        ManipulateCanvas(canvasHero2, false, false, 4);

        canvasHero1.gameObject.SetActive(true);
        canvasHero2.gameObject.SetActive(false);

    }

    private void Update()
    {
        if (step == 0)
        {
            // Dialogue et disparition dialogue
            if (!isStepDone)
            {
                DialogueRobot.instance.RobotSpeak("Welcome friends, it's been a long time since we worked together... Let's check the basis first.");
                isStepDone = true;
            }
            else if (TouchScreen() && DialogueRobot.instance.textEnded)
            {
                StartCoroutine(DialogueRobot.instance.iDisappear());
                step++;
                isStepDone = false;
            }
        return;
                
        } if(step == 1)
        {
            // Selection 1 case
            if (!isStepDone && !DialogueRobot.instance.isActive)
            {
              
                DialogueRobot.instance.RobotSpeak("I hope you at least remember how to move? If not, simply click on one of the available hexagones around you.");
                    
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
              return;
        }

        if(step == 2)
        {

                // Fin du tour
                if (!DialogueRobot.instance.isActive)
                {
                    DialogueRobot.instance.RobotSpeak("Perfect! But as you are heavily armed, Angelos, you cannot move and shoot on the same turn, so you might as well just press the end turn button.");
                    ManipulateCanvas(canvasHero1, true, false);
                    arrow.SetActive(true);
                    step++;
                }
            return;

        }

        if (step == 4)
            {
                if (!DialogueRobot.instance.isActive)
                {
                    DialogueRobot.instance.RobotSpeak("Now Spike, you can try to move on the green hexagone.");
                    step++;
                }
            return;

        }

        if (step == 5)
        {
            if (TouchTile(tileAimed) && DialogueRobot.instance.textEnded)
            {
                hero2.Move(tileAimed);
                   
                step++;
                StartCoroutine(DialogueRobot.instance.iDisappear());
            }
            return;

        }

        if (step == 6)
        {
            if (!DialogueRobot.instance.isActive)
            {
                DialogueRobot.instance.RobotSpeak("Contrary to this poor Angelos, you can attack and move on the same turn Spike! Blow this xeno hiding behind the rocks in front of you.");
                ManipulateCanvas(canvasHero2, false, true,1);
                step++;
            }
            return;

        }
        if (step == 7)
        {
            if (!boolean)
            {
                HexCoordinates coordinates = new HexCoordinates(3, 5);
                TilesManager.instance.mapTiles.TryGetValue(coordinates, out tileAimed);
                boolean = true;
            }

            TouchAimedTile(tileAimed, hero2);
            return;

        }
        if (step == 8)
        {
            if (!DialogueRobot.instance.isActive)
            {
                DialogueRobot.instance.RobotSpeak("Now that you have nothing left to do, you can simply pass your turn.");
                step++;
            }
            return;

        }

        // --------------------------------------------------------------------------------------------------------------------  NEW ROUND

        if (step == 10)
        {
            if (!DialogueRobot.instance.isActive)
            {
                boolean = false;
                DialogueRobot.instance.RobotSpeak("Now that these filthy xenos have played their turn, one of them just got in your range Angelos. Shoot him at will!");
                ManipulateCanvas(canvasHero1, false, true, 1);

            }
            else {
                if (!boolean)
                {
                    HexCoordinates coordinates = new HexCoordinates(4, 3);
                    TilesManager.instance.mapTiles.TryGetValue(coordinates, out tileAimed);
                    boolean = true;
                }
                    
                TouchAimedTile(tileAimed, hero1);
                   
            }
            return;

        }

        if (step == 11)
        {
            if (!DialogueRobot.instance.isActive)
            {
                DialogueRobot.instance.RobotSpeak("Now it's your turn to eradicate this filth Spike!");
                ManipulateCanvas(canvasHero1, true, false, 1);

            }
            return;

        }

        if (step == 12)
        {
            if (TouchTile(tileAimed))
            {
                hero2.Move(tileAimed);
                boolean = false;
                step++;
            }
            return;

        }

        if (step == 13)
        {
            if (!boolean)
            {
                boolean = true;
                HexCoordinates coordinates = new HexCoordinates(3, 4);
                TilesManager.instance.mapTiles.TryGetValue(coordinates, out tileAimed);
                ManipulateCanvas(canvasHero2, false, true, 1);
            }
            TouchAimedTile(tileAimed, hero2);
            return;
        }
        if(step == 14)
        {
            if (!DialogueRobot.instance.isActive)
            {
                DialogueRobot.instance.RobotSpeak("Nice! All that's left is their nest bursting babies. They are protected from distant attacks, so you will need to get close and attack them to destroy it. A mission will not be over until all of these are destroyed. Good luck! You are on your own now.");                         
            } 
            return;
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

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && DialogueRobot.instance.textEnded)
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
            print("wtf");
            nbrPassTurnHero1++;
            arrow.SetActive(false);
            StartCoroutine(DialogueRobot.instance.iDisappear());


            HexCoordinates coordinates = new HexCoordinates(hero2.myTile.coordinates.X + 1, hero2.myTile.coordinates.Z);
            TilesManager.instance.mapTiles.TryGetValue(coordinates, out tileAimed);
            tileAimed.SelectCell(HexCell.SELECTION_TYPE.MOVEMENT);
            ManipulateCanvas(canvasHero1, false);
            step++;

        }
        else if(nbrPassTurnHero1 == 1)
        {
            nbrPassTurnHero1++;
            HexCoordinates coordinates = new HexCoordinates(hero2.myTile.coordinates.X+1, hero2.myTile.coordinates.Z-1);
            TilesManager.instance.mapTiles.TryGetValue(coordinates, out tileAimed);
            tileAimed.SelectCell(HexCell.SELECTION_TYPE.MOVEMENT);
            ManipulateCanvas(canvasHero1, false);
            step++;
        }
    }

    public void TouchButtonPassTurnHero2()
    {
        if (nbrPassTurnHero2 == 0)
        {
            nbrPassTurnHero2++;
            StartCoroutine(DialogueRobot.instance.iDisappear());
            step++;

        }
        else if(nbrPassTurnHero2 == 1)
        {
            print("hello");
            nbrPassTurnHero2++;
            ManipulateCanvas(canvasHero1, true, true, 1);
            ManipulateCanvas(canvasHero2, true, true, 1);
            StartCoroutine(DialogueRobot.instance.iDisappear());
            lastOrder.SetActive(true);
            hero1.canPlay = true;
            hero2.canPlay = true;
            step++;

        }
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
        
        canvas.GetChild(0).GetChild(3).GetComponent<Button>().interactable = isPassTurnInteractable;
        if(!isPassTurnInteractable)
        canvas.GetChild(0).GetChild(3).GetComponent<Image>().color = Color.gray;
        else canvas.GetChild(0).GetChild(3).GetComponent<Image>().color = Color.white;
    }
}
