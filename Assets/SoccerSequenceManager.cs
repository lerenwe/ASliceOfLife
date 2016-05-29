using UnityEngine;
using System.Collections;

public class SoccerSequenceManager : MonoBehaviour {

    [SerializeField]
    GameObject soccerKid;
    NPC soccerKidScript;

    public static SoccerSequenceManager soccerSequenceMainScript;
    public GameObject Player;
    PlayerControls playerScript;
    public bool ballShot = false;
    public bool sequenceStartTriggered = false;
    public bool ballDisappeared;
    public bool waitingForDialogueEnd = false;
    public GameObject thisDialogue;

    [SerializeField]
    GameObject secondTargetPoint;

    // Use this for initialization
    void Start ()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerScript = Player.GetComponent<PlayerControls>();
        soccerSequenceMainScript = gameObject.GetComponent<SoccerSequenceManager>();

        if (soccerKid != null)
            soccerKidScript = soccerKid.GetComponent<NPC>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (ballShot)
        {
            playerScript.canControl = false;
        }

        if (ballDisappeared)
        {
            ballShot = false;
            soccerKidScript.moveTowardPlayer = true;
        }

        if (waitingForDialogueEnd && thisDialogue.GetComponent<Dialogue>().dialogueClosed)
        {
            waitingForDialogueEnd = false;

            Debug.Log("Going to point...");
            soccerKidScript.moveTowardTargetPoint = true;

            if (soccerKid != null && !soccerKidScript.GetComponent<Renderer>().isVisible)
            {
                Destroy(soccerKid.gameObject);
                playerScript.canControl = true;
            }
        }

        if (soccerKidScript != null && soccerKidScript.onDestinationPoint)
        {
            //Bon là faudra lancer le dialogue et tout freezer tant qu'il est pas fini, on peut peut être faire ça avec un yield... A voir !
            thisDialogue.GetComponent<Dialogue>().dialogueTriggered = true;
            Debug.Log("TRIGGERED HOLY SHIT");
            ballDisappeared = false;
            soccerKidScript.moveTowardPlayer = false;
            waitingForDialogueEnd = true;
            soccerKidScript.onDestinationPoint = false;
        }



	}
}
