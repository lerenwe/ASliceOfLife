using UnityEngine;
using System.Collections;
using System.IO;


[ExecuteInEditMode]
public class ChoiceLoader : MonoBehaviour {
    // Use this for initialization
    ChoicesContainer _everyChoices;
    public ChoicesContainer everyChoices;
    /*{
        get
        {
            return this._everyChoices;
        }
        set
        {
            this._everyChoices = value;
            Save();
        }
    }*/

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
        
    }
	
	public void Save ()
    {
        everyChoices.Save(Path.Combine(Application.dataPath, "choices.xml"));
    }
}
