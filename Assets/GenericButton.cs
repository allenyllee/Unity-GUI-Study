using UnityEngine;
using System.Collections;
public class GenericButton : MonoBehaviour {
	
    public Texture activeTexture; //texture to show when button is pressed (down)
    public IButton buttonScript;
	
	private Texture normalTexture;
	
	// Use this for initialization
	void Start () {
		normalTexture = guiTexture.texture;
	}

	// Update is called once per frame
	void Update ()
    {
       	bool mouseIntersect = guiTexture.HitTest(Input.mousePosition);
			
		// pressing down on button
		if (Input.GetMouseButtonDown(0) && mouseIntersect)
        {
            guiTexture.texture = activeTexture;
		}
		
		// releasing mouse
		if (Input.GetMouseButtonUp(0))
        {
            guiTexture.texture = normalTexture;
			
			// If releasing on the button, with a press that started on the button
			if (mouseIntersect && guiTexture.texture == activeTexture)
			{
				//Trigger button callback
				if(buttonScript) {
                	buttonScript.OnPressed();
				}
            }
        }
	}

}
