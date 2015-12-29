using UnityEngine;
using System.Collections;
using System.Linq;

public class SchoolSequence : MonoBehaviour
{

    GameObject GameStateManager;
    ChoiceLoader ChoiceScript;

    GameObject[] ExitZones;

    // Use this for initialization
    void Start()
    {
        GameStateManager = GameObject.Find("GameStateManager");
        ChoiceScript = GameStateManager.GetComponent<ChoiceLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ChoiceScript.everyChoices.choices.First(item => item.name == "Sporty or Nerdy").ChoiceMade == true)
        {
            if (ChoiceScript.everyChoices.choices.First(item => item.name == "Sporty or Nerdy").madeFirstChoice == true)
            {
                SetWalkWithThugs();
            }
            else
            {
                PlaceThugs();
            }
        }
    }

    void SetWalkWithThugs ()
    {

    }

    void PlaceThugs()
    {

    }
}
