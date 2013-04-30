using UnityEngine;
using System.Collections;

public class PlayerHUD : MonoBehaviour {
	
	// Public Classes
	public Texture WhiteTexture;
	
	public bool displayInfoBool = false;

	
	public const int LINE_HEIGHT = 20;
	public const int FLOATING_RECT_WIDTH = 200;
	public const int FLOATING_RECT_HEIGHT = 200;
	public const float FLOATING_RECT_OPACITY = 0.85f;
	
	// Update is called once per frame
	void Update () {		
		
		// Get Keyboard Input and Toggle floating box from it
		GetKeyboardInput();
	}
	
	// Get keyboard inputs to display floating menus
	void GetKeyboardInput()
	{
		if(Input.GetKeyDown (KeyCode.F1))
		{
			displayInfoBool = !displayInfoBool;	
		}
		
	}
	
	// Display GUI on screen
	void OnGUI()
	{		
		Screen.showCursor = false;	// Make the mouse cursor transparent
		DisplayStaticGUI();			// Display Button to toggle the Description HUD
		
		if(displayInfoBool)
		{	
			DisplayInfoHUD();		// Display the Description HUD
		}
	}

	// Contains all the Displaying of statics HUD
	private void DisplayStaticGUI()
	{ 
		float _buttonSizeX = Mathf.Min(Screen.width*0.30f, 150.0f);
		float _buttonSizeY = 20;
		float _buttonTop   = 5;
		float _buttonOffset = _buttonSizeX*0.1f;
		
		if(GUI.Button (new Rect(Screen.width - 1*(_buttonSizeX + _buttonOffset), _buttonTop, _buttonSizeX, _buttonSizeY), "F1-Show Help"      )){displayInfoBool = !displayInfoBool;}
	}
	
	private void DisplayInfoHUD()
	{
		float _posX  = 20;
		float _posY  = 30;
		float _sizeX = 180;
		float _sizeY = LINE_HEIGHT*6;
		float _opacity = FLOATING_RECT_OPACITY;
		
		DisplayTransparentBackground(_posX, _posY, _sizeX, _sizeY, _opacity);
		
		// Display string[] in FloatingLabels
		GUI.Label (new Rect(_posX, _posY + 0*LINE_HEIGHT , _sizeX, LINE_HEIGHT), "Culling : " + transform.GetComponent<CubeGeneration>().OcclusionCullingBool);
		GUI.Label (new Rect(_posX, _posY + 1*LINE_HEIGHT , _sizeX, LINE_HEIGHT), "Available   # of Cube : " + transform.GetComponent<CubeGeneration>().cubeAvailable);
		GUI.Label (new Rect(_posX, _posY + 2*LINE_HEIGHT , _sizeX, LINE_HEIGHT), "Total         # of Cube : " + transform.GetComponent<CubeGeneration>().cubeTotal    );
		GUI.Label (new Rect(_posX, _posY + 3*LINE_HEIGHT , _sizeX, LINE_HEIGHT), "Created     # of Cube : " + transform.GetComponent<CubeGeneration>().cubeRendered  );
		GUI.Label (new Rect(_posX, _posY + 4*LINE_HEIGHT , _sizeX, LINE_HEIGHT), "Occluded  # of Cube : " + transform.GetComponent<CubeGeneration>().cubeOccluded );
		
	}
	
	// Display a Transparent Background with parameters taken as input
	private void DisplayTransparentBackground(float _posX, float _posY, float _sizeX, float _sizeY, float _opacity)
	{
		Rect _objRect= new Rect(_posX, _posY, _sizeX, _sizeY);
		
		// Display semi-transparent background for processes and set color back to white
		GUI.color = new Color(0.0f, 0.0f, 0.0f, _opacity);
		GUI.DrawTexture(_objRect, WhiteTexture);
		GUI.color = Color.white;
	}
}