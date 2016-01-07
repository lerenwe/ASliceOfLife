using UnityEngine;
using System.Collections;

public class TriggerMomSequence : MonoBehaviour {

    [SerializeField]
    GameObject BedSceneManger;
    [SerializeField]
    GameObject momObject;
    TalkToParentsSequence parentSequenceScript;

    bool AlreadyTrigger = false;

    GameObject Player;
    PlayerControls playerScript;
    GameObject doorExit;

	// Use this for initialization
	void Start () {
        parentSequenceScript = BedSceneManger.GetComponent<TalkToParentsSequence>();

        Player = GameObject.Find("Player");
        playerScript = Player.GetComponent<PlayerControls>();
        doorExit = GameObject.Find("Exit_Door");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D (Collider2D hit)
    {
        if (hit.CompareTag ("Player") && !AlreadyTrigger)
        {
            Debug.Log("Triggah Triggah");
            StartCoroutine (AnswerToMom());
        }
    }

    IEnumerator AnswerToMom ()
    {
        momObject.GetComponent<SpriteRenderer>().enabled = true;
        playerScript.canControl = false;
        AlreadyTrigger = true;
        doorExit.GetComponent<Collider2D>().enabled = false;

        yield return StartCoroutine(WaitForKeyDown(KeyCode.Space));

        playerScript.canControl = true;
        momObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    IEnumerator WaitForKeyDown(KeyCode keyCode)
    {
        while (!Input.GetKeyDown(KeyCode.Space))
            yield return null;
    }
}
