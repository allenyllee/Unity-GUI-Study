using UnityEngine;
using System.Collections;

public class ScriptedGUI : MonoBehaviour {
	
	public Texture MenuBackground;
	public Texture Logo;
	public GUIStyle ButtonMenu;
	public GUIStyle ButtonStart;
	public GUIStyle ButtonNext;
	public GUIStyle ButtonPrevious;
	public GUIStyle ButtonQuit;
	
	void OnGUI() {
		float W = Screen.width;
		float H = Screen.height;
		
		//Menu background left
		GUI.DrawTexture( new Rect(-0.05f * W, -0.1f * H, 0.8f * W, 1.2f * H),
						 MenuBackground);
		
		//Menu background right
		GUI.DrawTexture( new Rect(0.7f * W, -0.1f * H, 0.3f * W, 1.2f * H),
						 MenuBackground);
		
		//Logo
		GUI.DrawTexture( new Rect(-0.35f * W, -0.46f * H, 1.0f * W, 1.0f * H),
						 Logo);
		
		// Menu
		GUI.Button( new Rect(0.76f * W, 0.03f * H, 0.18f * W, 0.14f * H),
					   "",
					   ButtonMenu);
		
		// Start
		GUI.Button( new Rect(0.76f * W, 0.18f * H, 0.18f * W, 0.14f * H),
					   "",
					   ButtonStart);
		
		// Menu
		GUI.Button( new Rect(0.76f * W, 0.33f * H, 0.18f * W, 0.14f * H),
					   "",
					   ButtonNext);
		
		// Previous
		GUI.Button( new Rect(0.76f * W, 0.48f * H, 0.18f * W, 0.14f * H),
					   "",
					   ButtonPrevious);
		
		// Quit
		GUI.Button( new Rect(0.76f * W, 0.83f * H, 0.18f * W, 0.14f * H),
					   "",
					   ButtonQuit);
	}
}
