using UnityEngine;
using System.Collections;

public class HexInfo : MonoBehaviour
{
	//Declarations
	public Vector3[] vertices;
	public Vector2[] uv;
	public int[] triangles;
	
	public Texture texture;
	
	// Use this for initialization
	void Start ()
	{
		MeshSetup ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	void MeshSetup ()
	{
		#region verts
		
		float floorLevel = 0;
		vertices = new Vector3[]
		{
			new Vector3 (-1f, floorLevel, -.5f),
			new Vector3 (-1f, floorLevel, .5f),
			new Vector3 (0f, floorLevel, 1f),
			new Vector3 (1f, floorLevel, .5f),
			new Vector3 (1f, floorLevel, -.5f),
			new Vector3 (0f, floorLevel, -1f)
		};
		
		#endregion
		
		#region triangles
		
		triangles = new int[]
		{
			1,5,0,
			1,4,5,
			1,2,4,
			2,3,4
		};
		
		#endregion
		
		#region uv
		
		uv = new Vector2[]
		{
			new Vector2 (0, 0.25f),
			new Vector2 (0, 0.75f),
			new Vector2 (0.5f, 1),
			new Vector2 (1, 0.75f),
			new Vector2 (1, 0.25f),
			new Vector2 (0.5f, 0)
		};
		
		#endregion
		
		#region finalise
		
		//add a mesh filter to the GameObject the script is attached to, cache it for later
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter> ();
		//add a mesh renderer to the GameObject the script is attached to
		gameObject.AddComponent<MeshRenderer> ();
		
		//create a mesh object to pass our data into
		Mesh mesh = new Mesh ();
		
		//add our vertices to the mesh
		mesh.vertices = vertices;
		//add ou triangles to the mesh
		mesh.triangles = triangles;
		//add our UV coordinates to the mesh
		mesh.uv = uv;
		
		//make it play nicely with lighting
		mesh.RecalculateNormals ();
		
		//set the GameObject's meshFilter's mesh to be the one we just made
		meshFilter.mesh = mesh;
		
		//UV TESTING
		renderer.material.mainTexture = texture;
		
		#endregion
	}
}
