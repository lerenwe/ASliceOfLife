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
    bool finishedCountingCharacters = false;
    public bool play = false;
    public bool finished = false;
    bool dirty;

    Vector2[] characterVertices = new Vector2[4];

    List<float> progress = new List<float>();
    List<float> previousProgress = new List<float>();
	List<Vector3> animStartPos = new List<Vector3> ();

    void Awake ()
	{
		textComponent = gameObject.GetComponent<Text> ();
		editorColor = textComponent.color;
        textComponent.color = startColorForText;

		#if UNITY_EDITOR
		if (!EditorApplication.isPlaying)
		{
			textComponent.color = editorColor;
		}
        #endif

        dirty = true;
	}

    void LateUpdate ()
	{
        if (finished && play)
        {
            progress.Clear();
            previousProgress.Clear();
            play = false;
        }

        if (play && !finished)
        {
            int iterator = 0;
            if (progress.Count > 0)
            {
                foreach (float charprogress in progress)
                {

                    if (iterator > 0 && charprogress <= 1 && progress[iterator - 1] >= timeBeforeNextChar)
                    {
                        progress[iterator] += speed * Time.deltaTime;

                        if (progress[iterator] != previousProgress[iterator])
                        {
                            previousProgress[iterator] = progress[iterator];
                            dirty = true;
                        }
                    }
                    else if (iterator == 0 && charprogress <= 1)
                    {
                        progress[iterator] += speed * Time.deltaTime;

                        if (progress[iterator] != previousProgress[iterator])
                        {
                            previousProgress[iterator] = progress[iterator];
                            dirty = true;
                        }
                    }

                    if (progress[iterator] > 1)
                        progress[iterator] = 1;

                    if (progress[progress.Count - 1] >= 1)
                    {
                        finished = true;
                        Debug.Log("Internal line display finished");
                    }

                    iterator++;
                }
            }

            if (dirty)
            {
                graphic.SetAllDirty();
                dirty = false;
            }
        }
	}

    public void Reset()
    {
        textComponent.color = startColorForText;
    }

    public override void ModifyMesh (VertexHelper vh)
    {
        #if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
        #endif
            if (play)
            {
                bool setStartPos = false;

                int verticesCount = vh.currentVertCount;
                int characterCount = vh.currentVertCount / 4;

                List<UIVertex> allVertices = new List<UIVertex>();

                if (animStartPos.Count != verticesCount)
                {
                    animStartPos.Clear();
                    setStartPos = true;
                    Debug.LogWarning("Updating vertices count");
                }
                else
                {
                    setStartPos = false;
                }

                if (progress.Count != characterCount)
                {
                    progress.Clear();
                    previousProgress.Clear();
                }

                while (progress.Count < characterCount)
                {
                    progress.Add(0);
                    previousProgress.Add(0);
                }

                int currentCount = 0;
                for (int i = 0; i < verticesCount; i += 4)
                {
                        for (int j = 0; j < 4; j++)
                        {
                            UIVertex uiVertex = new UIVertex();
                            vh.PopulateUIVertex(ref uiVertex, i + j);

                            if (setStartPos)
                            {
                                uiVertex.position += Vector3.up * displacementMultiplier;
                                animStartPos.Add(uiVertex.position);
                            }
                            else
                            {
                                uiVertex.position = animStartPos[i + j] - Vector3.up * progress[currentCount] * displacementMultiplier;
                                uiVertex.color = Color.Lerp(startColorForText, targetColorForText, progress[currentCount]);
                            }

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

