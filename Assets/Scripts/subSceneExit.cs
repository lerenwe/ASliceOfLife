using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class subSceneExit : MonoBehaviour {

    public GameObject destinationPoint;

    [SerializeField]
    bool touchToExit;
    bool playerIsTouchingExit = false;

    ChoiceLoader choiceLoader;

    [SerializeField]
    List<Choice> preRequisiteChoices = new List<Choice>();

    // Use this for initialization
    void Start () {
        choiceLoader = GameObject.Find("GameStateManager").GetComponent<ChoiceLoader>();
        //preRequisiteChoices = choiceLoader.everyChoices.choices;

        Debug.Log(preRequisiteChoices.GetHashCode());
        Debug.Log(choiceLoader.everyChoices.choices.GetHashCode());

        if (preRequisiteChoices.Count < choiceLoader.everyChoices.choices.Count)
        {
            foreach (Choice choice in choiceLoader.everyChoices.choices)
            {
                preRequisiteChoices.Add(choice);
            }
        }
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
	    if (!touchToExit && playerIsTouchingExit && Input.GetAxisRaw("Vertical") > .5f)
        {
            Debug.Log("Trying to exit...");

            if (preRequisiteChoices.SequenceEqual(choiceLoader.everyChoices.choices))
            { 
                GameStateManager.player.transform.position = destinationPoint.transform.position;
                Debug.Log("Exit right now");
            }
        }

        if (playerIsTouchingExit)
            Debug.Log("Player is colliding with " + gameObject.name);
	}

    void OnTriggerEnter2D (Collider2D hit)
    {
        if (touchToExit)
        {
            if (hit == GameStateManager.playerCollider)
                GameStateManager.player.transform.position = destinationPoint.transform.position;
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
}
