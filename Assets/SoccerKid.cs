using UnityEngine;
using System.Collections;

public class SoccerKid : NPC {

    void Awake ()
    {
        gameObject.GetComponent<Animator>().SetBool("HasBall", true);
    }
}
