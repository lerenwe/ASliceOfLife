using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressiveColor : BaseMeshEffect 
{
	public Color colorForText;
	public Color colorForTextDown;
	public float displacementMultiplier;

	private float internalTime;
	private bool isDirty;
	private float lastInternalTime;
	private float realTotalAnimationTime;

	private Text textComponent;
	private bool isPlaying;

	void Start ()
	{
		displacementMultiplier = 0;
		textComponent = gameObject.GetComponent<Text> ();
	}

	void Update ()
	{
		graphic.SetAllDirty ();
	}

	public override void ModifyMesh(VertexHelper vh)
	{
		int count = vh.currentVertCount;
		if (!IsActive() || count == 0 || isDirty)
		{
			vh.Clear();
			return;
		}

		int characterCount = 0;
		for (int i = 0; i < count; i += 4)
		{
			//CharacterData characterData = charactersData[characterCount];
			for (int j = 0; j < 4; j++)
			{
				UIVertex uiVertex = new UIVertex ();
				vh.PopulateUIVertex (ref uiVertex, i + j);
				if (j < 2) 
				{
					uiVertex.color = colorForText;
					uiVertex.position += Vector3.up * displacementMultiplier;
				} 
				else 
				{
					uiVertex.color = colorForTextDown;
				}
				vh.SetUIVertex (uiVertex, i + j);
			}
			characterCount += 1;
		}
		Debug.Log ("Character Count is = " + characterCount);
	}
}

