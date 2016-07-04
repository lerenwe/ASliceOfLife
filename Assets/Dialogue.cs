﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

    public List<DialogueCharacter> characters;
    public bool dialogueTriggered = false;
    public bool dialogueInProgress = false;
    public string[] dialogueLines;
    public int[] characterSpeaking;
    public GameObject[] dialogueCharacters;
    public bool dialogueClosed = false;

    int currentLineToDisplay = 0;

    DialogueDisplayer dialogueDisplayer;
    public GameObject dialogueDisplayObject;

	// Use this for initialization
	void Start ()
    {
        if (dialogueLines.Length != characterSpeaking.Length)
            Debug.LogError("THE NUMBER OF DIALOGUE LINES DOES NOT MATCH THE NUMBER OF CHARACTER SPEAKING");
	}
	
    public void TriggerDialogue ()
    {
        dialogueTriggered = true;
    }

	// Update is called once per frame
	void LateUpdate ()
    {
        if (!dialogueClosed)
        {
            if (dialogueTriggered || (dialogueInProgress && Input.GetKeyDown("space")))
            {
                dialogueDisplayer = dialogueDisplayObject.GetComponent<DialogueDisplayer>();
                dialogueDisplayObject.SetActive(true);

                dialogueDisplayer.InitializeNow();

                if (dialogueDisplayObject == null && dialogueDisplayObject.activeInHierarchy)
                    dialogueDisplayer = dialogueDisplayObject.GetComponent<DialogueDisplayer>();

                //The character's position is actually its upper sprite bound.
                //Vector3 characterPosition = Camera.main.WorldToScreenPoint(dialogueCharacters[characterSpeaking[currentLineToDisplay]].GetComponent<Collider2D>().bounds.max);
                Vector3 characterPosition = Camera.main.WorldToScreenPoint(dialogueCharacters[characterSpeaking[currentLineToDisplay]].transform.position);
                Vector3 characterMaxBounds = Camera.main.WorldToScreenPoint(dialogueCharacters[characterSpeaking[currentLineToDisplay]].GetComponent<SpriteRenderer>().bounds.max);



                Debug.Log("TRIGGERED & " + dialogueDisplayer.bubbleBackImage.sprite.bounds.size.y);
                //This is where we set up the position of the bubble according to the speaking character
                dialogueDisplayer.bubbleBackRectTransform.position = new Vector3 (characterPosition.x - dialogueDisplayer.bubbleBackRectTransform.rect.width / 2, characterPosition.y + characterMaxBounds.y / 2 + dialogueDisplayer.bubbleBackRectTransform.rect.height / 2, characterPosition.z);
                //FIX IT FELIX : A ce moment, dialogueDisplayer.bubbleBackImage est null. Trouve le moment d'initialisation dood.

                Debug.DrawLine(Camera.main.ScreenToWorldPoint(Vector3.zero), Camera.main.ScreenToWorldPoint(new Vector3(dialogueDisplayer.bubbleBackRectTransform.rect.xMax, dialogueDisplayer.bubbleBackRectTransform.rect.yMax, 0)), Color.green);
                Debug.Log("XMAX IS : " + dialogueDisplayer.bubbleBackRectTransform.transform.position.x + dialogueDisplayer.bubbleBackRectTransform.rect.size.x);

                if (dialogueDisplayer.bubbleBackRectTransform.transform.position.x + dialogueDisplayer.bubbleBackRectTransform.rect.size.x + 10 /*This is a small margin for the screen*/ > Screen.width)
                {
                    Debug.Log("Woops, I'm outta screen, lol");
                }

                dialogueDisplayer.textToDisplay = dialogueLines[currentLineToDisplay];
                dialogueDisplayer.ResetDialogueBubble();
                dialogueTriggered = false;
                dialogueInProgress = true;

                if (currentLineToDisplay < dialogueLines.Length - 1)
                    currentLineToDisplay++;
                else
                {
                    dialogueInProgress = false;
                    Debug.Log("Dialogue is over");
                }


            }
            else if (!dialogueInProgress && Input.GetKeyDown("space"))
            {
                Debug.Log("End of Dialogue");
                dialogueDisplayObject.SetActive(false);
                dialogueClosed = true;
            }
        }
	}
}
