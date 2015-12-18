using UnityEngine;
using System.Collections;
using System.IO;



public class ItemLoader : MonoBehaviour {

    public const string path = "items";

	// Use this for initialization
	void Start () 
    {
        ChoicesContainer everyChoices = ChoicesContainer.Load(Path.Combine(Application.dataPath, "items.xml"));

        foreach (Item choice in everyChoices.choices)
        {
            if (choice.name == "Sporty or Nerdy")
                choice.ChoiceMade = true;

            Debug.Log(choice.name);
            Debug.Log(choice.ChoiceMade);
        }

        everyChoices.Save(Path.Combine(Application.dataPath, "items.xml"));
        foreach (Item choice in everyChoices.choices)
        { 
            Debug.Log("AND NOW " + choice.name);
            Debug.Log("AND NOW " + choice.ChoiceMade);
        }
    }
	
	
}
