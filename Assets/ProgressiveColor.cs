using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressiveColor : BaseMeshEffect 
{
	Color colorForText;
    public Color startColorForText;
	public Color targetColorForText;
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
    bool initialize = true;

    Vector2[] characterVertices = new Vector2[4];

    List<float> progress = new List<float>();
    List<float> previousProgress = new List<float>();

    void Awake ()
	{
		displacementMultiplier = 0; // TODO: Should be displaced on start, THEN go to initial position, would be more logical
		textComponent = gameObject.GetComponent<Text> ();
        textComponent.color = startColorForText;
	}

	void Update ()
	{
        if (Input.GetKeyDown("w"))
        {
            play = true;
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

                    uiVertex.color = Color.Lerp(startColorForText, targetColorForText, progress[characterCount]);

                    uiVertex.position -= Vector3.up * progress[characterCount] * 10f;
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

		//Debug.Log ("Character Count is = " + characterCount + " & progress count is = " + progress.Count);
	}
}

