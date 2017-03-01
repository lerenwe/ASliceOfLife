using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BubbleText : BaseMeshEffect 
{
    public Color startColorForText;
	public Color targetColorForText;
	Color editorColor;
    public float speed;
	public float displacementMultiplier;
    public float timeBeforeNextChar = .15f;
	private Text textComponent;
    [SerializeField]
    public bool play = false;
    [HideInInspector]
    public bool finished = false;
    [HideInInspector]
    public bool initialize = false;
    int characterCount = 0;
    int verticesCount;
    public List<float> progress = new List<float>();
	List<Vector3> animStartPos = new List<Vector3> ();

    void Awake ()
	{
        //initialize = true;

		textComponent = gameObject.GetComponent<Text> ();
		editorColor = textComponent.color;
        textComponent.color = startColorForText;

		#if UNITY_EDITOR
		if (!EditorApplication.isPlaying)
		{
			textComponent.color = editorColor;
		}
        #endif
	}

    public void PlayNow ()
    {
        initialize = true;
        play = true;
    }

    void FixedUpdate ()
	{
        if (finished && play)
        {
            progress.Clear();
            play = false;
            //finished = false; //For making this script standalone, uncomment this line.
            initialize = true;
        }

        if (!finished && play && !initialize)
        {
            if (timeBeforeNextChar > 1)
                timeBeforeNextChar = 1;

            //Let's add the progress for each character now
            int iterator = 0;
            if (progress.Count == characterCount)
            {
                foreach (float charprogress in progress)
                {
                    if (progress[progress.Count - 1] >= 1)
                    {
                        finished = true;
                        break;
                    }

                    if (iterator == 0 && charprogress < 1)
                    {
                        progress[iterator] += speed * Time.deltaTime;

                        if (progress[iterator] > 1)
                            progress[iterator] = 1;
                    }
                    else if (iterator > 0 && charprogress < 1 && progress[iterator - 1] >= timeBeforeNextChar)
                    {
                        progress[iterator] += speed * Time.deltaTime;

                        if (progress[iterator] > 1)
                            progress[iterator] = 1;
                    }

                    iterator++;
                }
            }

            graphic.SetAllDirty();
        }

        
	}

    public void Reset()
    {
        textComponent.color = startColorForText;
    }

  

    void InitializeNow (VertexHelper vh)
    {
        animStartPos.Clear();

        verticesCount = vh.currentVertCount;
        characterCount = vh.currentVertCount / 4;

        while (progress.Count < characterCount)
        {
            progress.Add(0);
        }

        int currentCount = 0;
        for (int i = 0; i < verticesCount; i += 4)
        {
            for (int j = 0; j < 4; j++)
            {
                UIVertex uiVertex = new UIVertex();
                vh.PopulateUIVertex(ref uiVertex, i + j);

                uiVertex.position += Vector3.up * displacementMultiplier;
                animStartPos.Add(uiVertex.position);

                vh.SetUIVertex(uiVertex, i + j);
            }
            currentCount++;
        }

        initialize = false;
    }

    public override void ModifyMesh (VertexHelper vh)
    {
        #if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
        #endif

            if (play && initialize)
            {
                InitializeNow(vh);
            }
            else if (play && !initialize)
            {
                int currentCount = 0;
                for (int i = 0; i < verticesCount; i += 4)
                {
                        for (int j = 0; j < 4; j++)
                        {
                            UIVertex uiVertex = new UIVertex();
                            vh.PopulateUIVertex(ref uiVertex, i + j);

                            //Those two lines are where the magic is happening! Have fun with it ^^
                            uiVertex.position = animStartPos[i + j] - Vector3.up * progress[currentCount] * displacementMultiplier;
                            uiVertex.color = Color.Lerp(startColorForText, targetColorForText, progress[currentCount]);

                            vh.SetUIVertex(uiVertex, i + j);
                    }
                            currentCount++;
                }
            }

        #if UNITY_EDITOR
        }
        #endif
    }
}

