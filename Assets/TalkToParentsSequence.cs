using UnityEngine;
using System.Collections;
using System.Linq;

public class TalkToParentsSequence : MonoBehaviour {

    GameObject GameStateManager;
    ChoiceLoader ChoiceScript;

    GameObject[] ExitZones;

	// Use this for initialization
	void Start ()
    {
        GameStateManager = GameObject.Find("GameStateManager");
        ChoiceScript = GameStateManager.GetComponent<ChoiceLoader>();

        //foreach (Transform.child in )
        //ExitZones = 
	}
	
	// Update is called once per frame
	void Update () {
        if  (ChoiceScript.everyChoices.choices.First(item => item.name == "Defend or Ignore").ChoiceMade == true)
        {
            Debug.Log("Launch Parent Sequence");
        }
    }
}
