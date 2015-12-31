using UnityEngine;
using System.Collections;

public class BackGroundChangeSender : MonoBehaviour {

    [SerializeField]
    Sprite[] otherBackgrouds;
    [SerializeField]
    GameObject targetBackGroundObject;

    public void changeBackground (int backGroundNumber)
    {
        targetBackGroundObject.GetComponent<SpriteRenderer>().sprite = otherBackgrouds[backGroundNumber];
    }
}
