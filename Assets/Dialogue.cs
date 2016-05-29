using UnityEngine;
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
        dialogueDisplayer = dialogueDisplayObject.GetComponent<DialogueDisplayer>();
        if (dialogueLines.Length != characterSpeaking.Length)
            Debug.LogError("THE NUMBER OF DIALOGUE LINES DOES NOT MATCH THE NUMBER OF CHARACTER SPEAKING");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!dialogueClosed)
        {
            if (dialogueTriggered || (dialogueInProgress && Input.GetKeyDown("space")))
            {
                dialogueDisplayObject.SetActive(true);

                if (dialogueDisplayObject == null && dialogueDisplayObject.activeInHierarchy)
                    dialogueDisplayer = dialogueDisplayObject.GetComponent<DialogueDisplayer>();

                Vector3 characterPosition = Camera.main.WorldToScreenPoint(dialogueCharacters[characterSpeaking[currentLineToDisplay]].GetComponent<Collider2D>().bounds.max);


                dialogueDisplayer.bubbleBackRectTransform.position = new Vector3(characterPosition.x + dialogueDisplayer.bubbleBackRectTransform.GetComponent<Image>().rectTransform.rect.width / 2, characterPosition.y, characterPosition.z);

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
