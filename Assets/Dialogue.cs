using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialogue : MonoBehaviour {

    public List<DialogueCharacter> characters;
    public bool dialogueTriggered = false;
    public bool dialogueInProgress = false;

    int currentLineToDisplay = 0;

    DialogueDisplayer dialogueDisplayer;

	// Use this for initialization
	void Start () {
        dialogueDisplayer = GameObject.Find("DialogueDisplay").GetComponent<DialogueDisplayer>();
	}
	
	// Update is called once per frame
	void Update () {
	 if (dialogueTriggered ||(dialogueInProgress && Input.GetKeyDown ("space")))
        {
            dialogueDisplayer.textToDisplay = characters[0].dialogueLines[currentLineToDisplay];
            dialogueDisplayer.ResetDialogueBubble();
            dialogueTriggered = false;
            dialogueInProgress = true;

            if (currentLineToDisplay < characters[0].dialogueLines.Length - 1)
                currentLineToDisplay++;
            else
                dialogueInProgress = false;
        }
     else if (!dialogueInProgress && Input.GetKeyDown("space"))
        {
            Debug.Log("End of Dialogue");
        }
	}
}
