using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WordNextToFirstTest : MonoBehaviour {

    //public RectTransform otherWord;
    RectTransform mahRectTransform;
    public RectTransform previousWord;
    public float targetYPos = 0;
    Vector3 startPos = Vector3.zero;
    bool begin = true;

	// Use this for initialization
	void Start ()
    {
        mahRectTransform = gameObject.GetComponent<RectTransform>();
    }

    void Awake ()
    {
        
    }
	
	// Update is called once per frame
	void LateUpdate () {

        if(begin)
        {
            //Debug.Log(this.name + " " + mahRectTransform.position);
            startPos = mahRectTransform.position;

            if(previousWord != null) //If it's not the first word of this dialogue bubble
                mahRectTransform.position = new Vector3(mahRectTransform.position.x, mahRectTransform.position.y + 10, mahRectTransform.position.z);

            begin = false;
        }

        Vector3 velocityNow = Vector3.zero;

        if (previousWord != null)
        {
            Vector3 targetPos = new Vector3(previousWord.position.x, targetYPos, 0)
                    + new Vector3(previousWord.rect.xMax
                    + gameObject.GetComponent<RectTransform>().rect.size.x / 2, 0, 0);

            //if (Vector3.Distance(mahRectTransform.position, targetPos) < .1f)
                mahRectTransform.position = Vector3.SmoothDamp(mahRectTransform.position, targetPos, ref velocityNow, .08f);
            //else
                //mahRectTransform.position = targetPos;
        }
        //Debug.Log(this.name + " " + mahRectTransform.position);
        //mahRectTransform.position = otherWord.position + new Vector3(otherWord.rect.xMax + mahRectTransform.rect.size.x / 2, 0, 0);
    }
}
