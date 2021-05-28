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
    int indexDialogue = 0;

    [SerializeField]
    HeroController hero1, hero2;

    public Transform canvasHero1;
    public Transform canvasHero2;
    HexCell tileAimed = null;

    public GameObject arrow;

    private void Start()
    {
        hero1.canPlay = false;
        hero2.canPlay = false;
        TilesManager.instance.ClearTiles(false);

        ManipulateCanvas(canvasHero1, false, false);
        ManipulateCanvas(canvasHero2, false, false);

    }

    private void Update()
    {

        if (CombatSystem.instance.nbrRound != oldroundCount)
        {
            oldroundCount = CombatSystem.instance.nbrRound;
        }

        if(oldroundCount == 1)
        {
            if (indexDialogue == 0)
            {
                if (!isStepDone)
                {
                    DialogueRobot.instance.RobotSpeak("Salut cher ami, bienvenu dans ta mission d'éradication d'aliens");
                    isStepDone = true;
                }
                else if (TouchScreen() && DialogueRobot.instance.textEnded)
                {
                    StartCoroutine(DialogueRobot.instance.iDisappear());
                    indexDialogue++;
                    isStepDone = false;
                }
                
            } else if(indexDialogue == 1)
            {

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
                    indexDialogue++;
                    isStepDone = false;
                }
            }

            if(indexDialogue == 2)
            {
                if (!DialogueRobot.instance.isActive)
                {
                    StartCoroutine(RobotSpeakDelay("Parfait. Ton marine n'a désormais plus aucune action possible. Appuie sur ce bouton pour finir ton tour ", 0.5f));
                    ManipulateCanvas(canvasHero1, false, true);
                    arrow.SetActive(true);
                    indexDialogue++;
                }
            }

            if()
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
                    print("touchetile");
                    if (hitInformation.transform.GetComponent<HexCell>() == tile) return true;
                }
            }
        }
        return false;
    }

    bool TouchButton(Button button)
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
                    print("touchetile");
                    if (hitInformation.transform.GetComponent<HexCell>() == tile) return true;
                }
            }
        }
        return false;
    }

    IEnumerator RobotSpeakDelay(string sentence, float delay)
    {
        yield return new WaitForSeconds(delay);
        DialogueRobot.instance.RobotSpeak(sentence);
    }

    void ManipulateCanvas(Transform canvas, bool isActionsInteractable, bool isPassTurnInteractable)
    {
        foreach (Transform child in canvas.GetChild(0))
        {
            child.GetComponent<Button>().interactable = isActionsInteractable;
            if(!isActionsInteractable)
            child.GetComponent<Image>().color = Color.gray;
            else child.GetComponent<Image>().color = Color.white;
        }

        canvas.GetChild(1).GetComponent<Button>().interactable = isPassTurnInteractable;
        if(!isPassTurnInteractable)
        canvas.GetChild(1).GetComponent<Image>().color = Color.gray;
        else canvas.GetChild(1).GetComponent<Image>().color = Color.white;
    }
}
