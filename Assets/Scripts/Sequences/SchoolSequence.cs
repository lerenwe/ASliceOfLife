using UnityEngine;
using System.Collections;
using System.Linq;

public class SchoolSequence : MonoBehaviour
{

    GameObject GameStateManager;
    ChoiceLoader ChoiceScript;

    GameObject player;

    [SerializeField]
    GameObject Thug1;
    [SerializeField]
    GameObject Thug2;

    [SerializeField]
    GameObject targetPoint1;
    [SerializeField]
    GameObject targetPoint2;

    NPC Thug1Script;
    NPC Thug2Script;

    GameObject[] ExitZones;

    bool isTriggered = false;
    bool thugPlaced = false;

    // Use this for initialization
    void Start()
    {
        GameStateManager = GameObject.Find("GameStateManager");
        ChoiceScript = GameStateManager.GetComponent<ChoiceLoader>();

        Thug1Script = Thug1.GetComponent<NPC>();
        Thug2Script = Thug2.GetComponent<NPC>();

        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (ChoiceScript.everyChoices.choices.First(item => item.name == "Sporty or Nerdy").ChoiceMade == true && !isTriggered
            && CameraBehaviour.currentBackGround == this.GetComponent<SubSceneManager>().myBackGround)
        {
            if (ChoiceScript.everyChoices.choices.First(item => item.name == "Sporty or Nerdy").madeFirstChoice == true)
            {
                SetWalkWithThugs();
            }
            else if (ChoiceScript.everyChoices.choices.First(item => item.name == "Sporty or Nerdy").ChoiceMade == true
                    && ChoiceScript.everyChoices.choices.First(item => item.name == "Sporty or Nerdy").madeFirstChoice == false)
            {
                PlaceThugs();
            }
        }

        if (Mathf.Approximately( Mathf.Ceil (Thug2.transform.position.x),Mathf.Ceil (targetPoint2.transform.position.x)))
            Thug2.GetComponent<SpriteRenderer>().flipX = true;
    }

    void SetWalkWithThugs ()
    {
        Debug.Log("You'll walk among the thugs.");
        Thug1Script.moveTowardPlayer = true;
        Thug2Script.moveTowardPlayer = true;
    }

    void PlaceThugs()
    {
        Debug.Log("THUGS ARE WAITING FOR YOU");
        Thug1.transform.position = targetPoint1.transform.position;
        Thug2.transform.position = targetPoint2.transform.position;

        thugPlaced = true;
    }

    void OnTriggerEnter2D (Collider2D hit)
    {
        if (hit == player.GetComponent<Collider2D>() && !thugPlaced)
        {
            isTriggered = true;
            Debug.Log("Passed Nerd Point");

            Thug1Script.moveTowardPlayer = false;
            Thug2Script.moveTowardPlayer = false;

            Thug1Script.targetPoint = targetPoint1;
            Thug1Script.moveTowardTargetPoint = true;

            Thug2Script.targetPoint = targetPoint2;
            Thug2Script.moveTowardTargetPoint = true;
        }
    }
}
