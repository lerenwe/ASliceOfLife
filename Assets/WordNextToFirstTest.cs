using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WordNextToFirstTest : MonoBehaviour {

    //public RectTransform otherWord;
    public RectTransform previousWord;
    public float targetYPos = 0;
    public bool firstWord = false;
    public Dialogue parentDialogue;

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

                targetxPos = corners[1].x + mahRectTransform.rect.width * parentDialogue.canvas.scaleFactor / 2 + parentDialogue.xMargin;
                targetyPos = corners[1].y - mahRectTransform.rect.height * parentDialogue.canvas.scaleFactor / 2 - parentDialogue.yMargin;

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
        /*
                if (firstWord)
                {
                    Vector3[] corners = new Vector3[4];
                    parentDialogue.bubbleBackRectTransform.GetWorldCorners(corners);
                    Debug.Log(corners[0]);
                    float firstWordXPos = corners[1].x + mahRectTransform.rect.width * canvas.scaleFactor / 2 + parentDialogue.xMargin;
                    float firstWordYPos = corners[1].y - mahRectTransform.rect.height * canvas.scaleFactor / 2 - parentDialogue.yMargin;
                    Vector2 targetPosForFirstWord = new Vector2(firstWordXPos, firstWordYPos);

                    mahRectTransform.position = targetPosForFirstWord;
                }


                if (begin)
                {
                    startPos = mahRectTransform.position;

                    if(!firstWord && previousWord != null) //If it's not the first word of this dialogue bubble
                        mahRectTransform.position = new Vector3(mahRectTransform.position.x, mahRectTransform.position.y + 10, mahRectTransform.position.z);

                    begin = false;
                }

                Vector3 velocityNow = Vector3.zero;

                if (previousWord != null)
                {
                    Vector3 targetPos = new Vector3(previousWord.position.x, targetYPos, 0)
                            + new Vector3(previousWord.rect.width * canvas.scaleFactor / 2 
                            + gameObject.GetComponent<RectTransform>().rect.width * canvas.scaleFactor / 2, 0, 0);

                    mahRectTransform.position = Vector3.SmoothDamp(mahRectTransform.position, targetPos, ref velocityNow, .08f);
                }
                */
    }

    bool IsLineOverflowingHorizontally(float targetXPos, float targetYPos)
    {
        float currentWordXBoundary = targetXPos + mahRectTransform.rect.width * parentDialogue.canvas.scaleFactor / 2;
        float currentBubbleXBoundary = parentDialogue.bubbleBackRectTransform.position.x + parentDialogue.bubbleBackRectTransform.rect.width * parentDialogue.canvas.scaleFactor / 2;


        if (currentWordXBoundary > currentBubbleXBoundary)
        {
            Debug.Log("CLEARLY, YOU HAVE CROSSED THE LINE, BITCH");
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

            Debug.Log(this.name + " Line broke");
        }
        else
        {
            targetXPos = mahRectTransform.position.x;
            targetYPos = mahRectTransform.position.y;
        }

        parentDialogue.firstWordFromPreviousLine = mahRectTransform;
        brokeLineOnce = true;

        return new Vector2(targetXPos, targetYPos);

        /*targetYPos = firstWordYPos - previousWordRectTransform.rect.height * spawnedNewWord.GetComponent<WordNextToFirstTest>().canvas.scaleFactor / 2 - spaceBetweenLines;

spawnedNewWord.GetComponent<RectTransform>().position = new Vector3(firstWordXPos, targetYPos, 0);

firstWordXPos = spawnedNewWord.GetComponent<RectTransform>().position.x;
firstWordYPos = spawnedNewWord.GetComponent<RectTransform>().position.y;*/
    }
}
