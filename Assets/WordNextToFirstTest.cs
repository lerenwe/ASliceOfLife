using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WordNextToFirstTest : MonoBehaviour {

    //public RectTransform otherWord;
    RectTransform mahRectTransform;
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
            mahRectTransform.position = new Vector3(mahRectTransform.position.x, mahRectTransform.position.y + 10, mahRectTransform.position.z);
            //Debug.Log(this.name + " " + mahRectTransform.position);
            begin = false;
        }

        Vector3 velocityNow = Vector3.zero;
        mahRectTransform.position = Vector3.SmoothDamp(mahRectTransform.position, startPos, ref velocityNow, .08f);
        //Debug.Log(this.name + " " + mahRectTransform.position);
        //mahRectTransform.position = otherWord.position + new Vector3(otherWord.rect.xMax + mahRectTransform.rect.size.x / 2, 0, 0);
    }
}
