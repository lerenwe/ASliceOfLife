using UnityEngine;
using System.Collections;

public class ParentSequenceExit : subSceneExit
{

    [SerializeField]
    GameObject BedRoomManager;
    TalkToParentsSequence parentSequenceScript;


    public override void Exit()
    {
            base.Exit();
            parentSequenceScript = BedRoomManager.GetComponent<TalkToParentsSequence>();

            if (parentSequenceScript.triggeredSequence)
            {
                parentSequenceScript.sequenceEnded = true;
                Debug.Log("Ended parent sequence");
            }
    }
}
