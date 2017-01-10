using UnityEngine;
using System.Collections;

public class ParentSequenceExit : subSceneExit
{

    [SerializeField]
    GameObject BedRoomManager;

    TalkToParentsSequence parentSequenceScript;


    public override IEnumerator Exit()
    {
            if (destinationPoint.name == "Entry_Street")
            {
                SoundManager.OutsideAmbiance = true;
            }

            return base.Exit();

            parentSequenceScript = BedRoomManager.GetComponent<TalkToParentsSequence>();

            if (parentSequenceScript.triggeredSequence)
            {
                parentSequenceScript.sequenceEnded = true;
                Debug.Log("Ended parent sequence");
            }
    }
}
