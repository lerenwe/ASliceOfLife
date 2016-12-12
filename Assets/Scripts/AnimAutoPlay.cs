using UnityEngine;
using System.Collections;

public class AnimAutoPlay : MonoBehaviour {

    Animation animation;
    public float delayTime = 0f;

    float timeCount = 0;
    bool triggered = false;

	// Use this for initialization
	void Start ()
    {
        animation = transform.GetComponent<Animation>();

        if (delayTime <= 0)
            animation.Play();
	}
	

	// Update is called once per frame
	void Update ()
    {
        if (!triggered && delayTime > 0)
        {
            timeCount += Time.deltaTime;

            if (timeCount > delayTime)
            {
                animation.Play();
                triggered = true;
            }
        }
	}
}
