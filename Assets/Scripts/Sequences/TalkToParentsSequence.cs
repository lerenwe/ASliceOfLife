using UnityEngine;
using System.Collections;
using System.Linq;

public class TalkToParentsSequence : MonoBehaviour {

    GameObject GameStateManager;
    ChoiceLoader ChoiceScript;

    [SerializeField]
    GameObject[] ExitZones;
    [SerializeField]
    GameObject newDestinationPoint;
    [SerializeField]
    GameObject mom_Exit;

    [HideInInspector]
    public bool triggeredSequence = false;
    [HideInInspector]
    public bool sequenceEnded = false;

	// Use this for initialization
	void Start ()
    {
        GameStateManager = GameObject.Find("GameStateManager");
        ChoiceScript = GameStateManager.GetComponent<ChoiceLoader>();
	}
	
	// Update is called once per frame
	void Update () {
        if  (ChoiceScript.everyChoices.choices.First(item => item.name == "Defend or Ignore").ChoiceMade == true && !triggeredSequence)
        {
            Debug.Log("Launch Parent Sequence");
            triggeredSequence = true;
            mom_Exit.GetComponent<BoxCollider2D>().enabled = true;
            Debug.Log("Knock knock!");
        }

        if (sequenceEnded)
        {
            foreach (GameObject exit in ExitZones)
            {
                exit.GetComponent<subSceneExit>().destinationPoint = newDestinationPoint;
                Debug.Log("Exit to Dream Sequence set");
            }
        }
    }
}
