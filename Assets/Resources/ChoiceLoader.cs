using UnityEngine;
using System.Collections;
using System.IO;


[ExecuteInEditMode]
public class ChoiceLoader : MonoBehaviour {
    // Use this for initialization
    public ChoicesContainer everyChoices;

    void Start () 
    {
        everyChoices = ChoicesContainer.Load(Path.Combine(Application.dataPath, "choices.xml"));
        //everyChoices.Save(Path.Combine(Application.dataPath, "items.xml"));
        foreach (Choice choice in everyChoices.choices)
        {
            choice.ChoiceMade = false;
            choice.madeFirstChoice = false;
        }
    }

    void Update ()
    {
        everyChoices.Save(Path.Combine(Application.dataPath, "choices.xml"));
    }
	
	
}
