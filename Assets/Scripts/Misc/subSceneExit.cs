using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class subSceneExit : MonoBehaviour {

    public GameObject destinationPoint;

    [SerializeField]
    public bool touchToExit;
    [HideInInspector]
    public bool playerIsTouchingExit = false;
    bool preRequisitesMatch = false;

    ChoiceLoader choiceLoader;

    [SerializeField]
    List<Choice> preRequisiteChoices = new List<Choice>();

    [SerializeField]
    List<Choice> makeChoice = new List<Choice>();

    [Header("Change an exit's target when triggered")]
    [SerializeField]
    GameObject[] ExitToModify;
    [SerializeField]
    GameObject NewTargetPoint;

    [Header("Change a subScene background when trigered")]
    [SerializeField]
    BackGroundChangeReceptor targetBackGroundChangeScript;
    [SerializeField]
    int backGroundNumberToUse;

    // Use this for initialization
    void Start () {
        choiceLoader = GameObject.Find("GameStateManager").GetComponent<ChoiceLoader>();
        //preRequisiteChoices = choiceLoader.everyChoices.choices;

        //Debug.Log(preRequisiteChoices.GetHashCode());
        //Debug.Log(choiceLoader.everyChoices.choices.GetHashCode());

        #region Setting up local lists
        if (preRequisiteChoices.Count < choiceLoader.everyChoices.choices.Count)
        {
            foreach (Choice choice in choiceLoader.everyChoices.choices)
            {
                preRequisiteChoices.Add(choice);
            }
        }

        if (makeChoice.Count < choiceLoader.everyChoices.choices.Count)
        {
            foreach (Choice choice in choiceLoader.everyChoices.choices)
            {
                makeChoice.Add(choice);
            }
        }
        #endregion
    }

    void Awake ()
    {
        #region check if mandatory components & objects are present
        if (destinationPoint == null)
            Debug.LogError(gameObject.name + " needs a destination point!");
        #endregion
    }

    // Update is called once per frame
    void Update ()
    {
	    if (!touchToExit && playerIsTouchingExit && Input.GetAxisRaw("Vertical") > .5f && GameStateManager.player.GetComponent<PlayerControls>().canControl)
        {
            Debug.Log("Trying to exit...");

            int countChoiceCheck = 0;

            foreach (Choice choice in preRequisiteChoices)
            {
                if ( (choice.ChoiceMade == true && choiceLoader.everyChoices.choices[countChoiceCheck].ChoiceMade == choice.ChoiceMade) || choice.ChoiceMade == false)
                {
                    preRequisitesMatch = true;
                }
                else
                {
                    preRequisitesMatch = false;
                    break;
                }
                countChoiceCheck++;
            }

            if (preRequisitesMatch)
            {
                Debug.Log(gameObject.name + "'s prerequisites are matching.");
                Exit();
            }
            else
                Debug.Log(gameObject.name + "'s prerequisites are NOT matching.");

            countChoiceCheck = 0;
        }
	}

    public virtual void OnTriggerEnter2D (Collider2D hit)
    {
        if (touchToExit)
        {
            if (hit == GameStateManager.playerCollider)
                Exit();
        }
    }

    void OnTriggerStay2D (Collider2D hit)
    {
        if (!touchToExit)
        {
            if (hit == GameStateManager.playerCollider)
                playerIsTouchingExit = true;
        }
    }

    void OnTriggerExit2D (Collider2D hit)
    {
        if (!touchToExit)
        {
            if (hit == GameStateManager.playerCollider)
                playerIsTouchingExit = false;
        }
    }

    public virtual void Exit ()
    {
        //Update choice list
        int countChoiceCheck = 0;

        foreach (Choice choice in makeChoice)
        {
            if (choice.ChoiceMade == true && choice.ChoiceMade != choiceLoader.everyChoices.choices[countChoiceCheck].ChoiceMade)
                choiceLoader.everyChoices.choices[countChoiceCheck].ChoiceMade = choice.ChoiceMade;

            if (choice.madeFirstChoice == true && choice.madeFirstChoice != choiceLoader.everyChoices.choices[countChoiceCheck].madeFirstChoice)
                choiceLoader.everyChoices.choices[countChoiceCheck].madeFirstChoice = choice.madeFirstChoice;

            countChoiceCheck++;
        }

        countChoiceCheck = 0;

        #region If necessary, change an exit's target point
        foreach (GameObject exit in ExitToModify)
        {
            if (exit != null && NewTargetPoint != null)
                exit.GetComponent<subSceneExit>().destinationPoint = NewTargetPoint;
        }
        #endregion

        #region If necessary, change a background
        if (targetBackGroundChangeScript != null && backGroundNumberToUse != null)
            targetBackGroundChangeScript.changeBackground(backGroundNumberToUse);
        #endregion

        GameStateManager.player.transform.position = destinationPoint.transform.position;
    }
}
