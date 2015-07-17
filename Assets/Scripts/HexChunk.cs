using UnityEngine;
using System.Collections;

public class HexChunk : MonoBehaviour
{
	#region declarations
	[SerializeField]
	public HexInfo[,]
		hexArray;
	public int xSize;
	public int ySize;
	public Vector3 hexSize;

	//Set by world master
	public int xSector;
	public int ySector;
	public WorldManager worldManager;

	private MeshFilter filter;
	private new BoxCollider collider;
	#endregion

	public void SetSize (int x, int y)
	{
		xSize = x;
		ySize = y;
	}

	public void OnDestroy ()
	{
		Destroy (GetComponent<Renderer> ().material);
	}

	public void AllocateHexArray ()
	{
		hexArray = new HexInfo[xSize, ySize];
	}

	public void Begin ()
	{
		GenerateChunk ();
		for (int x = 0; x < xSize; x++) {
			for (int z = 0; z < ySize; z++) {
				if (hexArray [x, z] != null) {
					hexArray [x, z].parentChunk = this;
					hexArray [x, z].Start ();
				} else {
					print ("Null hexagon found in memory");
				}
			}
		}
		Combine ();
	}

	public void GenerateChunk ()
	{
		bool odd;

		for (int y = 0; y < ySize; y++) {
			odd = (y % 2) == 0;
			if (odd == true) {
				for (int x = 0; x < xSize; x++) {
					GenerateHex (x, y);
				}
			} else {
				for (int x = 0; x < xSize; x++) {
					GenerateHexOffset (x, y);
				}
			}
		}
	}

	public void GenerateHex (int x, int y)
	{
		//Cache and create hex
		HexInfo hex;
		Vector2 worldArrayPosition;
		hexArray [x, y] = new HexInfo ();
		hex = hexArray [x, y];

		//Set world array position for real texture positioning
		worldArrayPosition.x = x + (xSize * xSector);
		worldArrayPosition.y = y + (ySize * ySector);

		hex.CubeGridPosition = new Vector3 (worldArrayPosition.x - Mathf.Round ((worldArrayPosition.y / 2) + .1f), worldArrayPosition.y, - (worldArrayPosition.x - Mathf.Round ((worldArrayPosition.y / 2) + .1f) + worldArrayPosition.y));
		//Set local position of hex; This is the hex co-ord position local to the chunk
		hex.localPosition = new Vector3 ((x * (worldManager.hexExt.x * 2) + worldManager.hexExt.x), 0, (y * worldManager.hexExt.z) * 1.5f);
		//Set world position of hex; This is the hex co-ord position local to the world
		hex.worldPosition = new Vector3 (hex.localPosition.x + (xSector * (xSize * hexSize.x)), hex.localPosition.y, hex.localPosition.z + ((ySector * (ySize * hexSize.z)) * (.75f)));
		///Set hex values
		hex.hexExt = worldManager.hexExt;
		hex.hexCenter = worldManager.hexCenter;
	}

	public void GenerateHexOffset (int x, int y)
	{
		//Cache and create hex
		HexInfo hex;
		Vector2 worldArrayPosition;
		hexArray [x, y] = new HexInfo ();
		hex = hexArray [x, y];
		
		//Set world array position for real texture positioning
		worldArrayPosition.x = x + (xSize * xSector);
		worldArrayPosition.y = y + (ySize * ySector);
		
		hex.CubeGridPosition = new Vector3 (worldArrayPosition.x - Mathf.Round ((worldArrayPosition.y / 2) + .1f), worldArrayPosition.y, - (worldArrayPosition.x - Mathf.Round ((worldArrayPosition.y / 2) + .1f) + worldArrayPosition.y));
		//Set local position of hex; This is the hex co-ord position local to the chunk
		hex.localPosition = new Vector3 ((x * (worldManager.hexExt.x * 2)) + worldManager.hexExt.x, 0, (y * worldManager.hexExt.z) * 1.5f);
		//Set world position of hex; This is the hex co-ord position local to the world
		hex.worldPosition = new Vector3 (hex.localPosition.x + (xSector * (xSize * hexSize.x)), hex.localPosition.y, hex.localPosition.z + ((ySector * (ySize * hexSize.z)) * (.75f)));
		
		///Set hex values
		hex.hexExt = worldManager.hexExt;
		hex.hexCenter = worldManager.hexCenter;
	}

	private void Combine ()
	{
		CombineInstance[,] combine = new CombineInstance[xSize, ySize];

		for (int x = 0; x < xSize; x++) {
			for (int z = 0; z < ySize; z++) {
				combine [x, z].mesh = hexArray [x, z].localMesh;
				Matrix4x4 matrix = new Matrix4x4 ();
				matrix.SetTRS (hexArray [x, z].localPosition, Quaternion.identity, Vector3.one);
				combine [x, z].transform = matrix;
			}
		}

		filter = gameObject.GetComponent<MeshFilter> ();
		filter.mesh = new Mesh ();

		CombineInstance[] final;

		CivGridUtility.ToSingleArray (combine, out final);

		filter.mesh.CombineMeshes (final);
		filter.mesh.RecalculateNormals ();
		filter.mesh.RecalculateBounds ();
		MakeCollider ();
	}

	void MakeCollider ()
	{
		if (collider == null) {
			collider = gameObject.AddComponent<BoxCollider> ();
		}
		collider.center = filter.mesh.bounds.center;
		collider.size = filter.mesh.bounds.size;
	}
}
