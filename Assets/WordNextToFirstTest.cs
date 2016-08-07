using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WordNextToFirstTest : MonoBehaviour {

    //public RectTransform otherWord;
    public RectTransform previousWord;
    public float targetYPos = 0;
    public bool firstWord = false;
    public DialogueDisplayer parentDialogue;

    RectTransform mahRectTransform;
    public Vector3 startPos;
    bool secondUpdateCall = false;
    bool begin = true;
    bool brokeLineOnce = false;



	// Use this for initialization
	void Start ()
    {
        mahRectTransform = gameObject.GetComponent<RectTransform>();
        mahRectTransform.localScale = Vector3.one;
    }



    // Update is called once per frame
    void LateUpdate () {

        #region Set Word Start Position
        if (secondUpdateCall)
        {
            float targetxPos = 0f;
            float targetyPos = 0f;

            if (firstWord)
            {
                //We need to place the first word in the upper-left corner of the bubble, so let's get the corners first.
                Vector3[] corners = new Vector3[4];
                parentDialogue.bubbleBackRectTransform.GetWorldCorners(corners);

                targetxPos = corners[1].x + mahRectTransform.rect.width * parentDialogue.canvas.scaleFactor / 2 + parentDialogue.xMargin * parentDialogue.canvas.scaleFactor;
                targetyPos = corners[1].y - mahRectTransform.rect.height * parentDialogue.canvas.scaleFactor / 2 - parentDialogue.yMargin * parentDialogue.canvas.scaleFactor;

            if (begin)
                parentDialogue.firstWordFromPreviousLine = mahRectTransform;
            }
            else
            {
                targetxPos = previousWord.position.x + mahRectTransform.rect.width * parentDialogue.canvas.scaleFactor / 2 + previousWord.rect.width * parentDialogue.canvas.scaleFactor / 2;
                targetyPos = previousWord.position.y;
            }

            Vector2 targetStartPos = new Vector2(targetxPos, targetyPos);

            startPos = IsLineOverflowingHorizontally(targetxPos, targetyPos) ? BreakLine() : targetStartPos;
            #endregion

            mahRectTransform.position = startPos;

            begin = false;
        }
            secondUpdateCall = true;
    }

    bool IsLineOverflowingHorizontally(float targetXPos, float targetYPos)
    {
        float currentWordXBoundary = targetXPos + mahRectTransform.rect.width * parentDialogue.canvas.scaleFactor / 2;
        float currentBubbleXBoundary = parentDialogue.bubbleBackRectTransform.position.x + parentDialogue.bubbleBackRectTransform.rect.width * parentDialogue.canvas.scaleFactor / 2;


        if (currentWordXBoundary > currentBubbleXBoundary)
        {
            //Debug.Log("CLEARLY, YOU HAVE CROSSED THE LINE, BITCH");
            return true;

        }
        return false;
    }

    Vector2 BreakLine ()
    {
        float targetXPos;
        float targetYPos;

        if (!brokeLineOnce)
        {
            Vector3[] corners = new Vector3[4];
            parentDialogue.bubbleBackRectTransform.GetWorldCorners(corners);

            targetXPos = corners[1].x + mahRectTransform.rect.width * parentDialogue.canvas.scaleFactor / 2 + parentDialogue.xMargin;
            targetYPos = parentDialogue.firstWordFromPreviousLine.position.y - mahRectTransform.rect.height * parentDialogue.canvas.scaleFactor - parentDialogue.spaceBetweenLines;

            //Debug.Log(this.name + " Line broke");
        }
        else
        {
            targetXPos = mahRectTransform.position.x;
            targetYPos = mahRectTransform.position.y;
        }


        transform.SetParent(parentDialogue.bubbleBackRectTransform.transform);
        parentDialogue.firstWordFromPreviousLine = mahRectTransform;
        brokeLineOnce = true;

        return new Vector2(targetXPos, targetYPos);
    }
}
