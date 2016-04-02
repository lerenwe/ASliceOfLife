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
    bool begin = true;



	// Use this for initialization
	void Start ()
    {
        mahRectTransform = gameObject.GetComponent<RectTransform>();
        mahRectTransform.localScale = Vector3.one;
    }

    // Update is called once per frame
    void LateUpdate () {

        #region Set Word Start Position
        float xPos = 0f;
        float yPos = 0f;

        if (firstWord)
        {
            //We need to place the first word in the upper-left corner of the bubble, so let's get the corners first.
            Vector3[] corners = new Vector3[4];
            parentDialogue.bubbleBackRectTransform.GetWorldCorners(corners);

            xPos = corners[1].x + mahRectTransform.rect.width * parentDialogue.canvas.scaleFactor / 2 + parentDialogue.xMargin;
            yPos = corners[1].y - mahRectTransform.rect.height * parentDialogue.canvas.scaleFactor / 2 - parentDialogue.yMargin;
        }
        else
        {
            xPos = previousWord.position.x + mahRectTransform.rect.width * parentDialogue.canvas.scaleFactor / 2 + previousWord.rect.width * parentDialogue.canvas.scaleFactor / 2;
            yPos = previousWord.position.y;
        }

        Vector2 targetStartPos = new Vector2(xPos, yPos);
        startPos = targetStartPos;
        #endregion

        mahRectTransform.position = startPos;
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
}
