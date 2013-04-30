using UnityEngine;
using System.Collections;

public class CubeGeneration : MonoBehaviour {
	
	public int sizeX = 1;
	public int sizeY = 1;
	public int sizeZ = 1;
	
	public int cubeAvailable = 0;
	public int cubeTotal     = 0;
	public int cubeRendered   = 0;
	public int cubeOccluded  = 0;
	
	public GameObject CubeGrass;
	public GameObject CubeRock;
	public GameObject CubeSand;
	public bool OcclusionCullingBool = true;
	
	private Chunk _CurChunk;
	

	
	// Use this for initialization
	void Start () {
		
		// Create a Chunk
		_CurChunk = new Chunk();
		_CurChunk.Initialize();
		
	}
	
	void Update()
	{
	
		if(Input.GetKeyDown(KeyCode.Q))
		{
			RandomizeCurChunkHeightMap();
			AddChunk();
		}
		
		if(Input.GetKeyDown(KeyCode.E))
		{
			AddChunk();
		}
		
		if(Input.GetKeyDown(KeyCode.R))
		{
			RemoveChunk();
		}
		
		if(Input.GetKeyDown(KeyCode.Z))
		{
			OcclusionCullingBool = !OcclusionCullingBool;
			Debug.Log("Occlusion Culling = " + OcclusionCullingBool);
		}
		
	}
	
	void RemoveChunk()
	{
		GameObject[] _AllCube = GameObject.FindGameObjectsWithTag ("Cube");
		foreach(GameObject _Cube in _AllCube)
		{
			Destroy (_Cube);
		}
		
		cubeAvailable = 0;
		cubeTotal     = 0;
		cubeRendered   = 0;
		cubeOccluded  = 0;
		
	}
	
	void RandomizeCurChunkHeightMap()
	{
		int maxHeight = _CurChunk.sizeY;
		int octaveCount = 5;
		int _curHeight;
		
		float[][] perlinNoiseArray = new float[_CurChunk.sizeX][];
		
		// Calculate the perlinNoiseArray
		perlinNoiseArray = PerlinNoise.GeneratePerlinNoise(_CurChunk.sizeX,_CurChunk.sizeZ,octaveCount);
		
		for(int i = 0; i < _CurChunk.sizeX; i++)
		{
			for(int k = 0; k < _CurChunk.sizeZ; k++)
			{
				_curHeight = (int)(maxHeight*Mathf.Clamp01(perlinNoiseArray[i][k]));
			
				// Specify the type of block according to its height
				for(int j = 0; j < _curHeight; j++)
				{
					if(j == _curHeight - 1)
						_CurChunk.blocksArray[i,j,k] = 1; // Grass
					else if(j == _curHeight - 2)
						_CurChunk.blocksArray[i,j,k] = 3; // Sand
					else
						_CurChunk.blocksArray[i,j,k] = 2; // Rock
					
					if(j > 8)
						_CurChunk.blocksArray[i,j,k] = 2; // Grass
					
				}
			}	
		}
		
		
	}
	
	void AddChunk()
	{
		bool cubeClosedBool = false;
		
		cubeAvailable = _CurChunk.sizeX*_CurChunk.sizeY*_CurChunk.sizeZ;
		for(int i = 0; i < _CurChunk.sizeX; i++)
		{
			for(int j = 0; j < _CurChunk.sizeY; j++)
			{
				for(int k = 0; k < _CurChunk.sizeZ; k++)	
				{		
					cubeClosedBool = false;
					// Only draw the Cube if it's blocktype is not 0
					if(_CurChunk.blocksArray[i,j,k] != 0)
					{
						cubeTotal++;
						
						// Occlusion Culling
						if(OcclusionCullingBool == true)
						{
							
							//1st - Test if the Cube is on the limit of the chunk
							if((i > 0 && i < _CurChunk.sizeX-1) && (j > 0 && j < _CurChunk.sizeY-1) && (k > 0 && k < _CurChunk.sizeZ-1))
							{
								//2nd - Test if the cube is enclosed by other cubes
								if((_CurChunk.blocksArray[i+1,j,k] != 0 && _CurChunk.blocksArray[i-1,j,k] != 0 && _CurChunk.blocksArray[i,j+1,k] != 0 && _CurChunk.blocksArray[i,j-1,k] != 0 && _CurChunk.blocksArray[i,j,k+1] != 0 && _CurChunk.blocksArray[i,j,k-1] != 0 ))
								{
									///////Unrender object
									cubeClosedBool = true;
									Debug.Log ("Occluded");
									cubeOccluded++;
								}
							}
						}
					if(cubeClosedBool == false)
					{
						CreateCube(i,j,k, _CurChunk.blocksArray[i,j,k]);
						cubeRendered++;
					}
					
					}
				}
			}
		}
	}

	// Create a Cube at the inputted position with a texture matching the blockType
	void CreateCube(int _posX, int _posY, int _posZ, int _blockType)
	{
		GameObject _CurCube = CubeRock;  //Local variable that only remember the good prefab to be displayed
		// _blockType is inputted as an integer. 1 = Grass, 2 = Rock, 3 = Sand
		if(_blockType == 1)
			_CurCube = CubeGrass;		
		else if(_blockType == 2)
			_CurCube = CubeRock;
		else if(_blockType == 3)
			_CurCube = CubeSand;
		
		// Instantiate a new cube at the inputted position, facing identity
		GameObject.Instantiate(_CurCube, new Vector3(_posX, _posY, _posZ), Quaternion.identity);
	}
}
