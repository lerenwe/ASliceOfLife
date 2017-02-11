using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ProgressiveColor : BaseMeshEffect 
{
	Color colorForText;
    public Color startColorForText;
	public Color targetColorForText;
	Color editorColor;
    public float speed;
	public float displacementMultiplier;

	private float internalTime;
	private bool isDirty;
	private float lastInternalTime;
	private float realTotalAnimationTime;

	private Text textComponent;
	private bool isPlaying;
    bool finishedCountingCharacters = false;
    bool play = false;
    bool dirty = false;
	bool initialize = false;

    Vector2[] characterVertices = new Vector2[4];

    List<float> progress = new List<float>();
    List<float> previousProgress = new List<float>();
	List<Vector3> animStartPos = new List<Vector3> ();

    void Awake ()
	{
		displacementMultiplier = 0; // TODO: Should be displaced on start, THEN go to initial position, would be more logical
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

	void Update ()
	{
        if (Input.GetKeyDown("w"))
        {
            play = true;
			initialize = true;
        }

        if (play)
        {
            int iterator = 0;
            if (progress.Count > 0)
            {
                foreach (float charprogress in progress)
                {
                    if (iterator == 0 && charprogress <= 1)
                    {
                        progress[iterator] += speed * Time.deltaTime;

                        if (progress[iterator] != previousProgress[iterator])
                        {
                            previousProgress[iterator] = progress[iterator];
                            dirty = true;
                        }
                    }

                    if (iterator > 0 && charprogress <= 1 && progress[iterator - 1] >= .5f)
                    {
                        progress[iterator] += speed * Time.deltaTime;

                        if (progress[iterator] != previousProgress[iterator])
                        {
                            previousProgress[iterator] = progress[iterator];
                            dirty = true;
                        }
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

	public override void ModifyMesh(VertexHelper vh)
	{
		#if UNITY_EDITOR
		if(EditorApplication.isPlaying)
		{
		#endif
			
		int count = vh.currentVertCount;
		if (!IsActive() || count == 0 || isDirty)
		{
			vh.Clear();
			return;
		}

        List<CharacterVerticesPos> verticesPos = new List<CharacterVerticesPos>();

		int characterCount = 0;
		for (int i = 0; i < count; i += 4)
		{
			//CharacterData characterData = charactersData[characterCount];
			for (int j = 0; j < 4; j++)
			{
                if (finishedCountingCharacters) //TODO : J'en suis là.
                {
                    UIVertex uiVertex = new UIVertex();
                    vh.PopulateUIVertex(ref uiVertex, i + j);

					uiVertex.color = Color.Lerp (startColorForText, targetColorForText, progress [characterCount]);

					if (initialize)
						uiVertex.position += Vector3.up * 10f;
					else
						uiVertex.position = animStartPos[i+j] - Vector3.up * progress [characterCount] * 10f;

					if (initialize)
						animStartPos.Add (uiVertex.position);

                    vh.SetUIVertex(uiVertex, i + j);
                }
			}
			characterCount += 1;
        }

    finishedCountingCharacters = true;

        while (progress.Count < characterCount)
        {
            progress.Add(0);
            previousProgress.Add(0);
        }

		initialize = false;
		//Debug.Log ("Character Count is = " + characterCount + " & progress count is = " + progress.Count);
			#if UNITY_EDITOR
		}
			#endif
	}

}

