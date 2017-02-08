using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TextMeshModifier : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	static public Mesh TextGenToMesh(TextGenerator generator, Mesh mesh = null)
	{
		if (mesh == null)
			mesh = new Mesh();

		mesh.vertices = generator.verts.Select(v => v.position).ToArray();
		mesh.colors32 = generator.verts.Select(v => v.color).ToArray();
		mesh.uv = generator.verts.Select(v => v.uv0).ToArray();
		mesh.triangles = new int[generator.vertexCount * 6];
		for(var i = 0; i < mesh.triangles.Length; )
		{
			var t = i;
			mesh.triangles[i++] = t;
			mesh.triangles[i++] = t + 1;
			mesh.triangles[i++] = t + 2;
			mesh.triangles[i++] = t;
			mesh.triangles[i++] = t + 2;
			mesh.triangles[i++] = t + 3;
		}
		return mesh;
	}

}
