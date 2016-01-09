using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class NerdSequence : subSceneExit {

    [SerializeField]
    GameObject Nerd;

    bool isSporty = false;

    public override IEnumerator Exit ()
    {
        isSporty = false;

        foreach (Choice choice in choiceLoader.everyChoices.choices)
        {
            if (choice.name == "Sporty or Nerdy" && choice.madeFirstChoice == true) // If hero's sporty.
            {
                isSporty = true;
                break;
            }
        }

        Debug.Log("isSporty = " + isSporty);

        if (isSporty)
        {
            GameStateManager.player.GetComponent<PlayerControls>().canControl = false;

            GameStateManager.player.GetComponent<SpriteRenderer>().flipX = false;

            GameStateManager.player.GetComponent<Animator>().SetTrigger("Shove");
            Nerd.GetComponent<Animator>().SetTrigger("Fall");

            while (!GameStateManager.player.GetComponent<PlayerControls>().shoveAnimFinished)
                yield return null;

            StartCoroutine(base.Exit());
        }
        else
            StartCoroutine(base.Exit());
    }
}
