using UnityEngine;
using System.Collections;

public class CubeGeneration_V2 : MonoBehaviour {
	
	public GameObject CubeGrass;
	public GameObject CubeRock;
	public GameObject CubeSand;

	private Chunk _CurChunk;
	
	// Use this for initialization
	void Start () 
	{
		// Create a Chunk
		_CurChunk = new Chunk();	// Instantiate a new Chunk
		_CurChunk.Initialize();		// Initialize its blockArray
		FillChunk();				// Fill the _CurChunk with blocktype
		AddChunk();					// Add all the cube to the current scene
	}
	
	// Randomly fill the chunk
	void FillChunk()
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
	
	// Create a cube of the right type for each entry of the blocksArray
	void AddChunk()
	{
		for(int i = 0; i < _CurChunk.sizeX; i++)
		{
			for(int j = 0; j < _CurChunk.sizeY; j++)
			{
				for(int k = 0; k < _CurChunk.sizeZ; k++)	
				{
					if(_CurChunk.blocksArray[i,j,k] != 0)
					{
						CreateCube(i,j,k, _CurChunk.blocksArray[i,j,k]);
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
